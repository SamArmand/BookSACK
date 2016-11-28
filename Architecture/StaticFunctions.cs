using System;
using System.Collections.Generic;
using System.Linq;
using MysteriousDataProduct.Models;

namespace MysteriousDataProduct.Architecture
{
    public class StaticFunctions
    {

            public static Dictionary<string, int> GenerateSortedWordFrequency(string inputString)
            {
                
                // Create a new Dictionary object
                var dictionary = new Dictionary<string, int>();
                
                if (inputString == "" || inputString == null)
                    return dictionary;	

                    // Convert our input to lowercase
                inputString = inputString.ToLower();        
    
                // Define characters to strip from the input and do it
                string[] stripChars = { ";", ",", ".", "-", "_", "^", "(", ")", "[", "]", ":", "{", "}", "*", "!", "•", "—", "?", "\"", "~", "<", ">", "�",
                            "\n", "\t", "\r" };
                foreach (string character in stripChars)
                    inputString = inputString.Replace(character, " ");
                
                // Split on spaces into a List of strings
                var wordList = inputString.Split(' ').ToList();
    
                // Define and remove stopwords
                string[] stopwords = new string[] { "and", "the", "she", "for", "this", "you", "but", "these", "those", "they", "that" };
                foreach (string word in stopwords)
                {
                    // While there's still an instance of a stopword in the wordList, remove it.
                    // If we don't use a while loop on this each call to Remove simply removes a single
                    // instance of the stopword from our wordList, and we can't call Replace on the
                    // entire string (as opposed to the individual words in the string) as it's
                    // too indiscriminate (i.e. removing 'and' will turn words like 'bandage' into 'bdage'!)
                    while ( wordList.Contains(word) )
                        wordList.Remove(word);

                }
    
                // Loop over all over the words in our wordList...
                foreach (string word in wordList)
                {
                    // If the length of the word is at least three letters...
                    if (word.Length >= 3) 
                    {
                        // ...check if the dictionary already has the word.
                        if ( dictionary.ContainsKey(word) )
                            dictionary[word]++;
                            // If we already have the word in the dictionary, increment the count of how many times it appears

                        else
                            dictionary[word] = 1;
                            // Otherwise, if it's a new word then add it to the dictionary with an initial count of 1
                        
                    } // End of word length check
    
                } // End of loop over each word in our input
    
                // Create a dictionary sorted by value (i.e. how many times a word occurs)
                return (from entry in dictionary orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
    
            }

            /// <summary>
            /// Function that handles the logic of creating a TrainingBook object and inserting it into the database.
            /// </summary>
            /// <param name="synopsis">The summary of the new TrainingBook to be created.</param>
            /// <param name="subcategory">The subcategory of the new TrainingBook to be created.</param>
            /// <returns>Returns a new TrainingBook.</returns>
            public static TrainingBook CreateTrainingBook(string synopsis, string subcategory) {

                var trainingBook = new TrainingBook() {
                    Synopsis = synopsis,
                    Subcategory = subcategory
                };

                if (synopsis == null || synopsis == "")          
                    return trainingBook;

                return CreateTrainingBook(trainingBook);

            }


            public static TrainingBook CreateTrainingBook(TrainingBook trainingBook) 
            {

                var dataAccess = new DataAccess();

                var dictionary = dataAccess.GetDictionaries()[trainingBook.Subcategory];

                foreach (var kvp in trainingBook.SortedWordFrequency) {

                    if (!dictionary.ContainsKey(kvp.Key)) {

                        var Word = new Word() {

                            WordString = kvp.Key,
                            Subcategory = trainingBook.Subcategory,
                            FrequencyPlus1 = 1

                        };

                        dictionary.Add(Word.WordString, Word);

                    }

                    dictionary[kvp.Key].FrequencyPlus1 += kvp.Value;             

                }

                float sum = 0f;

                foreach (var kvp in dictionary)
                    sum += kvp.Value.FrequencyPlus1;

                foreach (var kvp in dictionary) {

                    kvp.Value.Probability = (float)kvp.Value.FrequencyPlus1 / sum;
                    kvp.Value.Ln = Math.Log(kvp.Value.Probability);

                }

                dataAccess.Update(trainingBook.Subcategory, dictionary);

                return trainingBook;

            }



    }

}
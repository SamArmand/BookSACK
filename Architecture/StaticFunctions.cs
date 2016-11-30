using System.Collections.Generic;
using System.Linq;
using MysteriousDataProduct.Models;

namespace MysteriousDataProduct.Architecture
{
    public class StaticFunctions
    {

            // Define characters to strip from the input and do it
            private static readonly string[] StripChars = { ";", ",", ".", "-", "_", "^", "(", ")", "[", "]", ":", "{", "}", "*", "!", "•", "—", "?", "\"", "~", "<", ">", "�", "'", "–", "|", "`", "/", "=", "+",
                "\n", "\t", "\r" };

            // Define and remove stopwords
            private static readonly string[] Stopwords = {"about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost", "alone", "along", "already", "also","although","always","among", "amongst", "amoungst", "amount", "and", "another", "any","anyhow","anyone","anything","anyway", "anywhere", "are", "around", "back","became", "because","become","becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom","but", "call", "can", "cannot", "cant", "con", "could", "couldnt", "cry", "describe", "detail", "done", "down", "due", "during", "each", "eight", "either", "eleven","else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "had", "has", "hasnt", "have", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "inc", "indeed", "interest", "into", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "off", "often", "once", "one", "only", "onto", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over", "own","part", "per", "perhaps", "please", "put", "rather", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thick", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru", "thus", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "under", "until", "upon", "very", "via", "was", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves"};
            

            public static Dictionary<string, int> GenerateSortedWordFrequency(string inputString)
            {
                
                // Create a new Dictionary object
                var dictionary = new Dictionary<string, int>();
                
                if (string.IsNullOrEmpty(inputString))
                    return dictionary;	

                    // Convert our input to lowercase
                inputString = inputString.ToLower();


                inputString = StripChars.Aggregate(inputString, (current, character) => current.Replace(character, " "));

                // Split on spaces into a List of strings
                var wordList = inputString.Split(' ').ToList();
    

                foreach (string word in Stopwords)
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
            /// <param name="subgenre">The subcategory of the new TrainingBook to be created.</param>
            /// <returns>Returns a new TrainingBook.</returns>
            public static TrainingBook CreateTrainingBook(string synopsis, string subgenre) {

                return CreateTrainingBook(new TrainingBook() {
                    Synopsis = synopsis,
                    Subgenre = subgenre
                });

            }

            public static TrainingBook CreateTrainingBook(TrainingBook trainingBook) 
            {

                var dataAccess = new DataAccess();

                var dictionary = dataAccess.GetDictionaries()[trainingBook.Subgenre];

                foreach (var kvp in trainingBook.SortedWordFrequency) {

                    if (!dictionary.ContainsKey(kvp.Key)) {

                        var word = new Word() {

                            WordString = kvp.Key,
                            Subgenre = trainingBook.Subgenre,
                            FrequencyPlus1 = 1

                        };

                        dictionary.Add(word.WordString, word);

                    }

                    dictionary[kvp.Key].FrequencyPlus1 += kvp.Value;             

                }

                var sum = dictionary.Aggregate(0f, (current, kvp) => current + kvp.Value.FrequencyPlus1);

                foreach (var kvp in dictionary)
                    kvp.Value.Probability = kvp.Value.FrequencyPlus1 / sum;
                

                dataAccess.Update(trainingBook.Subgenre, dictionary);

                return trainingBook;

            }



    }

}
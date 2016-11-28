using System.Collections.Generic;
using System.Linq;

public class StringProcessor
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

}
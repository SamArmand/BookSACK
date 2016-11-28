using System.Collections.Generic;
using System.Linq;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Models
{

    public class Book 
    {

        private string _summary;

        public string Summary 
        {
			get {return _summary;} 
			set {			
				_summary = value == null ? "" : value; 
				SortedWordFrequency = GenerateSortedWordFrequency(value);
				PredictSubcategory();
				}
        }

        private void PredictSubcategory()
        {
            if (SortedWordFrequency.Count == 0) {
				Subcategory = "Select one";
				return;
			}

			var dataAccess = new DataAccess();

			var dictionaries = dataAccess.GetDictionaries();

			var probabilities = new Dictionary<string,double>();

			probabilities.Add("Critically Acclaimed Mysteries Set in 1800–1950", 0);
			probabilities.Add("Female-Centered Murder Mysteries Set in 1914–1945 England", 0);
			probabilities.Add("Historical Thrillers About Books and Art", 0);
			probabilities.Add("Mysteries Set in Victorian and Edwardian England", 0);
			probabilities.Add("Occult and Political Mysteries Set in Medieval and Renaissance Europe", 0);

			foreach (var kvp in SortedWordFrequency) 
			{

				foreach(var kvp2 in probabilities) 
				{
					
					if (dictionaries[kvp2.Key].ContainsKey(kvp.Key))
						probabilities[kvp2.Key] += (SortedWordFrequency[kvp.Key] * dictionaries[kvp2.Key][kvp.Key].Probability);

				}

			}

			double max = 0;
			string maxSubcategory = "";

			foreach (var kvp in probabilities)
			{

				if (kvp.Value > max) 
				{
					max = kvp.Value;
					maxSubcategory = kvp.Key;
				}

			}

			Subcategory = maxSubcategory;			


        }

        public string Subcategory {get; set;}
		
		public Dictionary<string, int> SortedWordFrequency {get; set;}

        private Dictionary<string, int> GenerateSortedWordFrequency(string inputString)
        {
			
			// Create a new Dictionary object
			var dictionary = new Dictionary<string, int>();
			
			if (inputString == "" || inputString == null)
				return dictionary;	

            	// Convert our input to lowercase
			inputString = inputString.ToLower();        
 
			// Define characters to strip from the input and do it
			string[] stripChars = { ";", ",", ".", "-", "_", "^", "(", ")", "[", "]", ":", "{", "}", "*", "!", "•", "—", "?", "\"", "~", "<", ">",
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

}
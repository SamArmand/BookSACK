using System.Collections.Generic;
using System.Linq;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Models
{
    public class Book 
    {
        private string _synopsis = string.Empty;

        public string Synopsis 
        {
			get {return _synopsis;} 

			set 
			{				
				if (value != null) _synopsis = value; 
				
				var sortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);
				
				if (sortedWordFrequency.Count == 0)
					return;

				var dictionaries = DataAccess.GetDictionaries();

			    var probabilities = new Dictionary<string, double>
			    {
			        {"Critically Acclaimed Mysteries Set in 1800-1950", 0},
			        {"Female-Centered Murder Mysteries Set in 1914-1945 England", 0},
			        {"Historical Thrillers About Books and Art", 0},
			        {"Mysteries Set in Victorian and Edwardian England", 0},
			        {"Occult and Political Mysteries Set in Medieval and Renaissance Europe", 0}
			    };

			    foreach (var key1 in sortedWordFrequency.Keys.ToList())
					foreach(var key2 in probabilities.Keys.ToList()) 	
						if (dictionaries[key2].ContainsKey(key1))
							probabilities[key2] += (sortedWordFrequency[key1] * dictionaries[key2][key1].Probability);

				Subgenre = (from entry in probabilities orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value).First().Key;
			}
        }

        public string Subgenre {get; private set;} = string.Empty;
    }
}
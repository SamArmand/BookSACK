using System.Linq;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Models
{

    public class Book 
    {

        private string _synopsis;

        public string Synopsis 
        {
			get {return _synopsis;} 
			set {			
				_synopsis = value == null ? "" : value; 
				SortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);
				PredictSubcategory();
				}
        }

        private void PredictSubcategory()
        {
            if (SortedWordFrequency.Count == 0) {
				Subcategory = "";
				return;
			}

			var dataAccess = new DataAccess();

			var dictionaries = dataAccess.GetDictionaries();

			var probabilities = new Dictionary<string,double>();

			probabilities.Add("Critically Acclaimed Mysteries Set in 1800-1950", 0);
			probabilities.Add("Female-Centered Murder Mysteries Set in 1914-1945 England", 0);
			probabilities.Add("Historical Thrillers About Books and Art", 0);
			probabilities.Add("Mysteries Set in Victorian and Edwardian England", 0);
			probabilities.Add("Occult and Political Mysteries Set in Medieval and Renaissance Europe", 0);

			foreach (var key1 in SortedWordFrequency.Keys.ToList()) 
			{

				foreach(var key2 in probabilities.Keys.ToList()) 
				{
					
					if (dictionaries[key2].ContainsKey(key1))
						probabilities[key2] += (SortedWordFrequency[key1] * dictionaries[key2][key1].Probability);

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

    }

}
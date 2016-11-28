using System.Collections.Generic;

namespace MysteriousDataProduct.Models
{

    public class TrainingBook 
    {

        private string _synopsis;

        public string Synopsis
        {
			get {return _synopsis;} 
			set { 
				_synopsis = value == null ? "" : value; 
				SortedWordFrequency = StringProcessor.GenerateSortedWordFrequency(value);
				}
        }

        public string Subcategory {get; set;}
		
		public Dictionary<string, int> SortedWordFrequency {get; set;}

    }

}
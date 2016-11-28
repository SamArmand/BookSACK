using System.Collections.Generic;

namespace MysteriousDataProduct.Models
{

    public class TrainingBook 
    {

        private string _summary;

        public string Summary
        {
			get {return _summary;} 
			set { 
				_summary = value == null ? "" : value; 
				SortedWordFrequency = StringProcessor.GenerateSortedWordFrequency(value);
				}
        }

        public string Subcategory {get; set;}
		
		public Dictionary<string, int> SortedWordFrequency {get; set;}

    }

}
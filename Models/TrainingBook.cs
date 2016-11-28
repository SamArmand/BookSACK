using System.Collections.Generic;
using MysteriousDataProduct.Architecture;

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
				SortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);
				}
        }

        public string Subcategory {get; set;}
		
		public Dictionary<string, int> SortedWordFrequency {get; set;}

    }

}
using System.Collections.Generic;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Models
{

    public class TrainingBook 
    {

        private string _synopsis = string.Empty;

        public string Synopsis
        {
			get {return _synopsis;} 
			set {
                if (value != null) _synopsis = value; 
				SortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);
				}
        }

        public string Subgenre {get; set;}
		
		public Dictionary<string, int> SortedWordFrequency {get; set;}

    }

}
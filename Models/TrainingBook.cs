using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Models
{
    public class TrainingBook 
    {
        bool _processed;

        private string _synopsis = string.Empty;
        private string _subgenre = string.Empty;

        public string Synopsis
        {
			get {return _synopsis;} 
			set 
            {
                if (value != null) _synopsis = value; 
				SortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);

                if (!_processed && _subgenre != string.Empty) Process();
            }
        }

        public string Subgenre 
        {
            get {return _subgenre;} 

            set 
            {
                if (value != null) _subgenre = value;
                if (!_processed && _synopsis != string.Empty) Process();
            }
        }
		
		public Dictionary<string, int> SortedWordFrequency {get; set;} = new Dictionary<string, int>();

        private void Process() 
        {
            var dictionary = DataAccess.GetDictionaries()[Subgenre];

            foreach (var kvp in SortedWordFrequency) 
            {
                if (!dictionary.ContainsKey(kvp.Key))
                    dictionary.Add(kvp.Key, new Word()
                    {
                        WordString = kvp.Key,
                        Subgenre = Subgenre,
                    });

                dictionary[kvp.Key].FrequencyPlus1 += kvp.Value;             
            }

            var sum = dictionary.Aggregate(0f, (current, kvp) => current + kvp.Value.FrequencyPlus1);

            foreach (var kvp in dictionary) kvp.Value.Probability = kvp.Value.FrequencyPlus1 / sum;

            DataAccess.Update(Subgenre, dictionary);

            _processed = true;

        }

        public HtmlString TableForView() 
		{
			if (string.IsNullOrEmpty(_synopsis)) return HtmlString.Empty;

			StringBuilder sb = (new StringBuilder())
				.Append("<table class='table'><thead class='thead-inverse'><tr><th>Word</th><th>Frequency</th></tr></thead><tbody>");

			foreach (var word in SortedWordFrequency) 
				sb.Append("<tr><td>").Append(word.Key).Append("</td><td>")
					.Append(word.Value).Append("</td></tr>");

			return new HtmlString(sb.Append("</tbody></table>").ToString());
		}
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
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

			    foreach (var key1 in sortedWordFrequency.Keys.ToList())
					foreach(var key2 in Probabilities.Keys.ToList()) 	
						if (dictionaries[key2].ContainsKey(key1))
							Probabilities[key2] += (sortedWordFrequency[key1] * dictionaries[key2][key1].Probability);

				Probabilities = (from entry in Probabilities orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

				Subgenre = Probabilities.First().Key;

			}
        }

        public string Subgenre {get; private set;} = string.Empty;

		public Dictionary<string, double> Probabilities {get; private set;} = new Dictionary<string, double>
		{
			{"Critically Acclaimed Mysteries Set in 1800-1950", 0},
			{"Female-Centered Murder Mysteries Set in 1914-1945 England", 0},
			{"Historical Thrillers About Books and Art", 0},
			{"Mysteries Set in Victorian and Edwardian England", 0},
			{"Occult and Political Mysteries Set in Medieval and Renaissance Europe", 0}
		};

		public HtmlString TableForView() 
		{
			if (string.IsNullOrEmpty(_synopsis)) return HtmlString.Empty;

			bool first = true;
			StringBuilder sb = (new StringBuilder())
				.Append("<table class='table'><thead class='thead-inverse'><tr><th>Subgenre</th><th>Probability Sum</th></tr></thead><tbody>");

			foreach (var probability in Probabilities) 
			{
				sb.Append(first ? "<tr class='table-success'><td><strong>" : "<tr><td>")
					.Append(probability.Key).Append(first ? "</strong></td><td><strong>": "</td><td>")
					.Append(probability.Value).Append(first ? "</strong></td></tr>" : "</td></tr>");
				first = false;
			}

			return new HtmlString(sb.Append("</tbody></table>").ToString());
		}
    }
}
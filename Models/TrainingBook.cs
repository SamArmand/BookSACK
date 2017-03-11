using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using BookSack.Architecture;

namespace BookSack.Models
{
    /// <summary>
    /// Model for a Training Book
    /// </summary>
    public class TrainingBook 
    {
        /// <summary>
        /// Determines if the Process() function has been executed
        /// </summary>
        private bool _processed;

        /// <summary>
        /// Private field for the synopsis
        /// </summary>
        private string _synopsis = string.Empty;
        
        /// <summary>
        /// Private field for the subgenre
        /// </summary>
        private string _subgenre = string.Empty;

        /// <summary>
        /// Property for the synopsis
        /// </summary>
        /// <returns>The book's synopsis</returns>
        public string Synopsis
        {
            /// <summary>
            /// Getter for the synopsis property
            /// </summary>
			get {return _synopsis;} 
			
            /// <summary>
            /// Setter for the synopsis property
            /// </summary>
            set 
            {
                if (value != null) _synopsis = value;

                // Get the sorted work frequency for this synopsis
				SortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);

                // If both the subgenre and synopsis are set, start the training process
                if (!_processed && _subgenre != string.Empty) Process();
            }
        }

        /// <summary>
        /// Property for the subgenre
        /// </summary>
        /// <returns>The book's subgenre</returns>
        public string Subgenre 
        {
            /// <summary>
            /// Getter for the Subgenre property
            /// </summary>
            get {return _subgenre;} 

            /// <summary>
            /// Setter for the Subgenre property
            /// </summary>
            set 
            {
                if (value != null) _subgenre = value;
                if (!_processed && _synopsis != string.Empty) Process();
            }
        }
		
        /// <summary>
        /// Property for the sorted word frequency dictionary
        /// </summary>
        /// <returns>The book's sorted word frequency dictionary. Defaults to an empty dictionary</returns>
		public Dictionary<string, int> SortedWordFrequency {get; private set;} = new Dictionary<string, int>();

        /// <summary>
        /// Private function to process the training book and execute the training of the model
        /// </summary>
        private void Process()
        {
            //Get the dictionary for this subgenre
            var dictionary = DataAccess.GetDictionaries()[Subgenre];

            // foreach word in the synopsis...
            foreach (var kvp in SortedWordFrequency)
            {
                // ...if the subgenre's dictionary doesn't contain this word yet...
                if (!dictionary.ContainsKey(kvp.Key))
                    // ...add it
                    dictionary.Add(kvp.Key, new Word()
                    {
                        WordString = kvp.Key,
                        Subgenre = Subgenre,
                    });

                // ...increment its dictionary entry with the amount of times it appears in this synopsis
                dictionary[kvp.Key].FrequencyPlus1 += kvp.Value;             
            }

            // sum up all the frequencies in the subgenre dictionary
            var sum = dictionary.Aggregate(0f, (current, kvp) => current + kvp.Value.FrequencyPlus1);

            // for each word in the subgenre dictionary, divide its frequency by the sum of frequencies to find its probability
            foreach (var kvp in dictionary) kvp.Value.Probability = kvp.Value.FrequencyPlus1 / sum;

            // update the subgenre dictionary
            DataAccess.Update(dictionary);

            _processed = true;
        }

		/// <summary>
        /// Creates the results table in the view
        /// </summary>
        /// <returns>An HtmlString to be passed to the view for rendering</returns>
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
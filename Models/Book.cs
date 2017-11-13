using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using BookSack.Architecture;

namespace BookSack.Models
{
    /// <summary>
    /// Model class for the Book class
    /// </summary>
    public sealed class Book
    {
        /// <summary>
        /// Private field representing the book's synopsis
        /// </summary>
        private string _synopsis = string.Empty;

        /// <summary>
        /// Property for the synopsis
        /// </summary>
        /// <returns>Returns the synopsis</returns>
        public string Synopsis
        {
            get => _synopsis;

            internal set
            {
                if (value != null) _synopsis = value;

                // Parse the synopsis in a dictionary sorted by frequency
                var sortedWordFrequency = StaticFunctions.GenerateSortedWordFrequency(value);

                // If no valid words are found, stop the process
                if (sortedWordFrequency.Count == 0) return;

                // Get dictionaries from the database
                var dictionaries = DataAccess.GetDictionaries();

                // for each word in the synopsis dictionary
                foreach (var key1 in sortedWordFrequency.Keys.ToList())
                    // ...and for each subgenre
                foreach (var key2 in Probabilities.Keys.ToList().Where(key2 => dictionaries[key2].ContainsKey(key1)))
                    Probabilities[key2] += (sortedWordFrequency[key1] * dictionaries[key2][key1].Probability);

                // Sort the probabilities in descending order
                Probabilities = (from entry in Probabilities orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                // Set the subgenre to the subgenre with the max probability
                Subgenre = Probabilities.First().Key;
            }
        }

        /// <summary>
        /// Propery for the subgenre. Defaults to an empty string
        /// </summary>
        /// <returns></returns>
        internal string Subgenre { get; private set; } = string.Empty;

        /// <summary>
        /// Property for the Dictionary of probabilities. Defaults to a dictionary of zeros.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, double> Probabilities { get; set; } = new Dictionary<string, double>
        {
            {"Critically Acclaimed Mysteries Set in 1800-1950", 0},
            {"Female-Centered Murder Mysteries Set in 1914-1945 England", 0},
            {"Historical Thrillers About Books and Art", 0},
            {"Mysteries Set in Victorian and Edwardian England", 0},
            {"Occult and Political Mysteries Set in Medieval and Renaissance Europe", 0}
        };

        /// <summary>
        /// Creates the results table in the view
        /// </summary>
        /// <returns>An HtmlString to be passed to the view for rendering</returns>
        public HtmlString TableForView()
        {
            if (string.IsNullOrEmpty(_synopsis)) return HtmlString.Empty;

            var first = true;
            var sb = new StringBuilder("<table class='table'><thead class='thead-inverse'><tr><th>Subgenre</th><th>Probability Sum</th></tr></thead><tbody>");

            foreach (var probability in Probabilities)
            {
                sb.Append(first ? "<tr class='table-success'><td><strong>" : "<tr><td>")
                    .Append(probability.Key).Append(first ? "</strong></td><td><strong>" : "</td><td>")
                    .Append(probability.Value).Append(first ? "</strong></td></tr>" : "</td></tr>");
                first = false;
            }

            return new HtmlString(sb.Append("</tbody></table>").ToString());
        }
    }
}
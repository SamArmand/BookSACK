using System.Collections.Generic;
using System.Linq;

namespace MysteriousDataProduct.Architecture
{
    /// <summary>
    /// A class to store static functions
    /// </summary>
    internal class StaticFunctions
    {

        internal static string ConnectionString = "";

        // Define characters to strip from the input
        private static readonly string[] StripChars =
        {
            ";", ",", ".", "-", "_", "^", "(", ")", "[", "]", ":", "{", "}", "*", "!", "•", "—", "?", "\"", "~",
            "<", ">", "�", "'", "–", "|", "`", "/", "=", "+",
            "\n", "\t", "\r"
        };
        
        // Define stopwords
        private static readonly string[] Stopwords =
        {
            "about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost",
            "alone", "along", "already", "also", "although", "always", "among", "amongst", "amoungst", "amount",
            "and", "another", "any", "anyhow", "anyone", "anything", "anyway", "anywhere", "are", "around",

            "back", "became", "because","become","becomes", "becoming", "been", "before", "beforehand", "behind",
            "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom", "but",

            "call", "can", "cannot", "cant", "con", "could", "couldnt", "cry",

            "describe", "detail", "done", "down", "due", "during",

            "each", "eight", "either", "eleven", "else", "elsewhere", "empty", "enough", "etc", "even", "ever",
            "every", "everyone", "everything", "everywhere", "except",

            "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty",
            "found", "four", "from", "front", "full", "further",

            "get", "give",

            "had", "has", "hasnt", "have", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon",
            "hers", "herself", "him", "himself", "his", "how", "however", "hundred",

            "inc", "indeed", "interest", "into", "its", "itself",

            "keep",

            "last", "latter", "latterly", "least", "less", "ltd",

            "made", "many", "may", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly",
            "move", "much", "must", "myself",

            "name", "namely", "neither", "never", "nevertheless", "next", "nine", "nobody", "none", "noone", "nor",
            "not", "nothing", "now", "nowhere",

            "off", "often", "once", "one", "only", "onto", "other", "others", "otherwise", "our", "ours",
            "ourselves", "out", "over", "own",

            "part", "per", "perhaps", "please", "put",

            "rather",

            "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show",
            "side", "since", "sincere", "six", "sixty", "some", "somehow", "someone", "something", "sometime",
            "sometimes", "somewhere", "still", "such", "system",

            "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there",
            "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thick", "thin", "third",
            "this", "those", "though", "three", "through", "throughout", "thru", "thus", "together", "too", "top",
            "toward", "towards", "twelve", "twenty", "two",

            "under", "until", "upon",

            "very", "via",

            "was", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter",
            "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who",
            "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would",

            "yet", "you", "your", "yours", "yourself", "yourselves"
        };
        
        /// <summary>
        /// Parses the synopsis into a sorted dictionary
        /// </summary>
        /// <param name="synopsis">The synopsis to parse</param>
        /// <returns></returns>
        internal static Dictionary<string, int> GenerateSortedWordFrequency(string synopsis)
        {    
            // Create a new Dictionary object
            var dictionary = new Dictionary<string, int>();
            
            // If no valid synopsis was provided, just return the empty dictionary
            if (string.IsNullOrEmpty(synopsis)) return dictionary;

            // Convert our input to lowercase [ synopsis.ToLower() passed as seed to Aggregate(...) function ]
            // Remove special characters [ StripChars.Aggregate(...) ]
            // Split on spaces into array [ Split(' ') ]
            // Keep only words at least 3 characters long that are not stopwords [ Where(...) ]
            // Loop over all over the words in the resulting collection... [ foreach(...) ]
            // Check if the dictionary already has the word. [ dictionary.ContainsKey(word) ? ]
            // If it does, increment the count of how many times it appears...
            // ...otherwise, if it's a new word then add it to the dictionary with an initial count of 1 [ dictionary[word] + 1 : 1 ]
            // All in one line of code... wow
            foreach (var word in 
                (StripChars.Aggregate(synopsis.ToLower(), (current, stripChar) => current.Replace(stripChar, " ")))
                    .Split(' ')
                    .Where(w => w.Length >= 3 && !Stopwords.Contains(w)))
                dictionary[word] = dictionary.ContainsKey(word) ? dictionary[word] + 1 : 1;

            // Create a dictionary sorted by value (i.e. how many times a word occurs)
            return (from entry in dictionary orderby entry.Value descending select entry)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
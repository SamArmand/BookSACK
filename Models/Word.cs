namespace BookSack.Models
{
    /// <summary>
    /// Model for a word
    /// </summary>
    internal sealed class Word {

        /// <summary>
        /// Property for the actual word
        /// </summary>
        /// <returns>The actual word as a string</returns>
        public string WordString {get; set;}

        /// <summary>
        /// Property for the subgenre the word belongs to
        /// </summary>
        /// <returns>The subgenre it belongs to as a string</returns>
        public string Subgenre {get; set;}

        /// <summary>
        /// Property for the word's frequency + 1 in the subgenre dictionary. Defaults to 1 for smoothing
        /// </summary>
        /// <returns>The frequency + 1 as an int</returns>
        public int FrequencyPlus1 {get; set;} = 1;

        /// <summary>
        /// Property for the word's probability of appearing in the subgenre dictionary
        /// </summary>
        /// <returns></returns>
        public double Probability {get; set;}
    }
}


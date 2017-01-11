using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MysteriousDataProduct.Models;

namespace MysteriousDataProduct.Architecture
{
    /// <summary>
    /// The class that deals with database access
    /// </summary>
    internal class DataAccess
    {
        /// <summary>
        /// Gets the dictionaries for all subgenres in the database
        /// </summary>
        /// <returns>A Dictionary of the dictionaries of all subgenres in the database</returns>
        internal static Dictionary<string, Dictionary<string, Word>> GetDictionaries()
        {
            // Initialize the Dictionary with empty dictionaries for each subgenre
            var dictionaries = new Dictionary<string, Dictionary<string, Word>>
            {
                {"Critically Acclaimed Mysteries Set in 1800-1950", new Dictionary<string, Word>()},
                {"Female-Centered Murder Mysteries Set in 1914-1945 England", new Dictionary<string, Word>()},
                {"Historical Thrillers About Books and Art", new Dictionary<string, Word>()},
                {"Mysteries Set in Victorian and Edwardian England", new Dictionary<string, Word>()},
                {
                    "Occult and Political Mysteries Set in Medieval and Renaissance Europe",
                    new Dictionary<string, Word>()
                }
            };

            var command = new SqlCommand("SELECT WordString, Subgenre, FrequencyPlus1, Probability FROM Dictionaries;");

            Open(command);

            var reader = command.ExecuteReader();

            // For each returned word from the database
            while (reader.Read())
            {
                // Create a Word object
                var word = new Word
                {
                    WordString = reader.GetString(reader.GetOrdinal("WordString")),
                    Subgenre = reader.GetString(reader.GetOrdinal("Subgenre")),
                    FrequencyPlus1 = reader.GetInt32(reader.GetOrdinal("FrequencyPlus1")),
                    Probability = Convert.ToDouble(reader.GetFloat(reader.GetOrdinal("Probability")))
                };

                // Insert the Word object in the dictionary of the subgenre it is found in
                dictionaries[word.Subgenre].Add(word.WordString, word);
            }

            // Dispose of all database connection objects
            reader.Dispose();
            Dispose(command);

            return dictionaries;
        }

        /// <summary>
        /// A function that resets the database
        /// </summary>
        internal static void Reset() => Execute(new SqlCommand("DELETE FROM Dictionaries;"));

        /// <summary>
        /// Updates a dictionary in the database
        /// </summary>
        /// <param name="dictionary">The dictionary to be updated</param>
        internal static void Update(Dictionary<string, Word> dictionary)
        {
            var command = new SqlCommand("DELETE FROM Dictionaries WHERE Subgenre=@Subgenre;");

            command.Parameters.AddWithValue("@Subgenre", dictionary.Keys.First());

            Execute(command);

            // Insert each word from the dictionary into the database
            foreach (var word in dictionary.Values)
            {
                command = new SqlCommand("INSERT INTO Dictionaries (WordString, Subgenre, FrequencyPlus1, Probability) VALUES (@WordString, @Subgenre, @FrequencyPlus1, @Probability);");

                command.Parameters.AddWithValue("@WordString", word.WordString);
                command.Parameters.AddWithValue("@Subgenre", word.Subgenre);
                command.Parameters.AddWithValue("@FrequencyPlus1", word.FrequencyPlus1);
                command.Parameters.AddWithValue("@Probability", word.Probability);

                Execute(command);
            }
        }

        /// <summary>
        /// Executes the SqlCommand
        /// </summary>
        /// <param name="command">The SqlCommand to execute</param>
        private static void Execute(SqlCommand command)
        {
            Open(command);
            command.ExecuteNonQuery();
            Dispose(command);
        }

        /// <summary>
        /// Opens a connection with the SqlCommand
        /// </summary>
        /// <param name="command">The SqlCommand for which to open a connection</param>
        private static void Open(SqlCommand command)
        {
            command.Connection = new SqlConnection((new AppSettings()).MS_TableConnectionString);
            command.Connection.Open();
        }

        /// <summary>
        /// Disposes the SqlCommand and its connection
        /// </summary>
        /// <param name="command">The SqlCommand to dispose along with its connection</param>
        private static void Dispose(SqlCommand command)
        {
            command.Connection.Dispose();
            command.Dispose();
        }
    }
}
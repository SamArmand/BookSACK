using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MysteriousDataProduct.Models;

namespace MysteriousDataProduct.Architecture
{
    internal class DataAccess
    {

        private const string ConnectionString = "Server=tcp:h98ohmld2f.database.windows.net,1433;Database=BookSACK;User Id=JMSXTech@h98ohmld2f;Password=jmsx!2014;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        internal Dictionary<string, Dictionary<string, Word>> GetDictionaries()
        {

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

            while (reader.Read())
            {

                var word = new Word
                {
                    WordString = reader.GetString(reader.GetOrdinal("WordString")),
                    Subgenre = reader.GetString(reader.GetOrdinal("Subgenre")),
                    FrequencyPlus1 = reader.GetInt32(reader.GetOrdinal("FrequencyPlus1")),
                    Probability = Convert.ToDouble(reader.GetFloat(reader.GetOrdinal("Probability")))
                };


                dictionaries[word.Subgenre].Add(word.WordString, word);

            }

            reader.Dispose();

            Dispose(command);

            return dictionaries;
        }

        internal bool Reset()
        {

            Execute(new SqlCommand("DELETE FROM Dictionaries;"));

            return true;

        }

        internal void Update(string subcategory, Dictionary<string, Word> dictionary)
        {

            var command = new SqlCommand("DELETE FROM Dictionaries WHERE Subgenre=@Subgenre;");

            command.Parameters.AddWithValue("@Subgenre", subcategory);

            Execute(command);

            foreach (var kvp in dictionary)
                Insert(kvp.Value);

        }

        public void Insert(Word word)
        {
            var command = new SqlCommand("INSERT INTO Dictionaries (WordString, Subgenre, FrequencyPlus1, Probability) VALUES (@WordString, @Subgenre, @FrequencyPlus1, @Probability);");

            command.Parameters.AddWithValue("@WordString", word.WordString);
            command.Parameters.AddWithValue("@Subgenre", word.Subgenre);
            command.Parameters.AddWithValue("@FrequencyPlus1", word.FrequencyPlus1);
            command.Parameters.AddWithValue("@Probability", word.Probability);

            Execute(command);
        }

        private static void Execute(SqlCommand command)
        {

            Open(command);
            command.ExecuteNonQuery();
            Dispose(command);

        }

        private static void Open(SqlCommand command)
        {
            command.Connection = new SqlConnection(ConnectionString);
            command.Connection.Open();

        }

        private static void Dispose(SqlCommand command)
        {
            command.Connection.Dispose();
            command.Dispose();
        }

    }
}
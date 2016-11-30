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

            var dictionaries = new Dictionary<string, Dictionary<string, Word>>();

            dictionaries.Add("Critically Acclaimed Mysteries Set in 1800-1950", new Dictionary<string,Word>());
            dictionaries.Add("Female-Centered Murder Mysteries Set in 1914-1945 England", new Dictionary<string,Word>());
            dictionaries.Add("Historical Thrillers About Books and Art", new Dictionary<string,Word>());
            dictionaries.Add("Mysteries Set in Victorian and Edwardian England", new Dictionary<string,Word>());
            dictionaries.Add("Occult and Political Mysteries Set in Medieval and Renaissance Europe", new Dictionary<string,Word>());

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT WordString, Subgenre, FrequencyPlus1, Probability FROM Dictionaries");


            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                      
                var word = new Word() {};

                word.WordString = reader.GetString(reader.GetOrdinal("WordString"));
                word.Subgenre = reader.GetString(reader.GetOrdinal("Subgenre"));
                word.FrequencyPlus1 = reader.GetInt32(reader.GetOrdinal("FrequencyPlus1"));
                word.Probability = Convert.ToDouble(reader.GetFloat(reader.GetOrdinal("Probability")));

                dictionaries[word.Subgenre].Add(word.WordString, word);

            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return dictionaries;
        }

        internal bool Reset()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("DELETE FROM Dictionaries;");

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            return true;

        }

        internal void Update(string subcategory, Dictionary<string, Word> dictionary)
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("DELETE FROM Dictionaries WHERE Subgenre=@Subgenre;");

            command.Parameters.AddWithValue("@Subgenre", subcategory);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            foreach (var kvp in dictionary)
                Insert(kvp.Value);

        }

        public void Insert(Word word)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("INSERT INTO Dictionaries (WordString, Subgenre, FrequencyPlus1, Probability) VALUES (@WordString, @Subgenre, @FrequencyPlus1, @Probability);");

            command.Parameters.AddWithValue("@WordString", word.WordString);
            command.Parameters.AddWithValue("@Subgenre", word.Subgenre);
            command.Parameters.AddWithValue("@FrequencyPlus1", word.FrequencyPlus1);
            command.Parameters.AddWithValue("@Probability", word.Probability);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

    }
}
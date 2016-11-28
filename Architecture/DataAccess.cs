using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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

            var command = new SqlCommand("SELECT WordString, Subcategory, FrequencyPlus1, Probability, Ln FROM Dictionaries");


            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                
                
                var word = new Word() {};

                    word.FrequencyPlus1 = reader.GetInt32(reader.GetOrdinal("FrequencyPlus1"));
                    word.Probability = Convert.ToDouble(reader.GetFloat(reader.GetOrdinal("Probability")));
                    word.Ln = Convert.ToDouble(reader.GetFloat(reader.GetOrdinal("Ln")));
                word.WordString = reader.GetString(reader.GetOrdinal("WordString"));
                word.Subcategory = reader.GetString(reader.GetOrdinal("Subcategory"));
                
                dictionaries[word.Subcategory].Add(word.WordString, word);


            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return dictionaries;
        }

        internal void Update(string subcategory, Dictionary<string, Word> dictionary)
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("DELETE FROM Dictionaries WHERE Subcategory=@Subcategory;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Subcategory", subcategory);

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

            var command = new SqlCommand("INSERT INTO Dictionaries (WordString, Subcategory, FrequencyPlus1, Probability, Ln) VALUES (@WordString, @Subcategory, @FrequencyPlus1, @Probability, @Ln);") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@WordString", word.WordString);
            command.Parameters.AddWithValue("@Subcategory", word.Subcategory);
            command.Parameters.AddWithValue("@FrequencyPlus1", word.FrequencyPlus1);
            command.Parameters.AddWithValue("@Probability", word.Probability);
            command.Parameters.AddWithValue("@Ln", word.Ln);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }


    }
}
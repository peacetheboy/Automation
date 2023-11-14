using Newtonsoft.Json.Linq;
using System.Data.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;

namespace AutomationPrj
{
    public class ReadData
    {
        static void Main()
        {
            try
            {
                // Step 1: Read data from a JSON file
                string jsonFilePath = "C:\\Users\\itstudent\\source\\repos\\Automation"; // Replace with your JSON file path
                string jsonContent = File.ReadAllText(jsonFilePath);

                // Create an SQL table
                string connectionString = "Integrated Security=SSPI;Persist Security Info=False;User ID=\"\";Initial Catalog=CCGDB; TrustServerCertificate=true; Data Source=\"\""; // Replace with your SQL Server connection string
                string tableName = "Table"; 
                CreateTable(connectionString, tableName);

                // Step 3: Import the JSON data into the SQL table
                ImportJsonDataToSql(jsonContent, connectionString, tableName);

                // Step 4: Delete the original JSON file after successful insertion
                File.Delete(jsonFilePath);

                Console.WriteLine("Data import successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        // Function to create an SQL table
        static void CreateTable(string connectionString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"CREATE TABLE {tableName} (name VARCHAR(255), phone VARCHAR(10), email VARCHAR(255), address VARCHAR(255), postalZip VARCHAR(255), region VARCHAR(255), country VARCHAR(255), list VARCHAR(255), numberrange VARCHAR(255), currency VARCHAR(255), alphanumeric VARCHAR(255)", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Function to import JSON data into SQL table
        static void ImportJsonDataToSql(string jsonContent, string connectionString, string tableName)
        {
            JArray jsonArray = JArray.Parse(jsonContent);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (JObject jsonObject in jsonArray)
                {
                    using (SqlCommand command = new SqlCommand($"INSERT INTO {tableName} (name, phone, email, address, postalZip, region, country, list, numberrange, currency, alphanumeric ) VALUES (@name, @phone, @email, @address, @postalZip, @region, @country, @list, @numberrange, @currency, @alphanumeric)", connection))
                    {
                        command.Parameters.AddWithValue("@name", (int)jsonObject["name"]);
                        command.Parameters.AddWithValue("@phone", (string)jsonObject["phone"]);
                        command.Parameters.AddWithValue("@email", (int)jsonObject["email"]);
                        command.Parameters.AddWithValue("@address", (int)jsonObject["address"]);
                        command.Parameters.AddWithValue("@postalZip", (string)jsonObject["postalZip"]);
                        command.Parameters.AddWithValue("@region", (int)jsonObject["region"]);
                        command.Parameters.AddWithValue("@country", (string)jsonObject["country"]);
                        command.Parameters.AddWithValue("@list", (int)jsonObject["list"]);
                        command.Parameters.AddWithValue("@numberrange", (int)jsonObject["numberrange"]);
                        command.Parameters.AddWithValue("@currency", (string)jsonObject["currency"]);
                        command.Parameters.AddWithValue("@alphanumeric", (int)jsonObject["alphanumeric"]);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

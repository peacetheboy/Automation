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
                string jsonFilePath = "C:\\Users\\user\\Documents\\GitHub\\Automation-PROJECT"; // Replace with your JSON file path
                string jsonContent = File.ReadAllText(jsonFilePath);

                // Create an SQL table
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MYDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; // Replace with your SQL Server connection string
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

                using (SqlCommand command = new SqlCommand($"CREATE TABLE {tableName} (Id INT PRIMARY KEY, Name NVARCHAR(255), Age INT)", connection))
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
                    using (SqlCommand command = new SqlCommand($"INSERT INTO {tableName} (Id, Name, Age) VALUES (@Id, @Name, @Age)", connection))
                    {
                        command.Parameters.AddWithValue("@Id", (int)jsonObject["Id"]);
                        command.Parameters.AddWithValue("@Name", (string)jsonObject["Name"]);
                        command.Parameters.AddWithValue("@Age", (int)jsonObject["Age"]);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

/* 
Author: Sergey Makarov
Date: 11/21/24
Assignment: Programming Assignment 6-2
*/

// Import the libraries
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace FactoryModel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Factory Model Example\r\n");

            //Pull the provider and connection string info and get it to show up in the command line 
            var (provider, connectionString) = GetProviderFromConfiguation();
            Console.WriteLine($"Provider: {provider}");
            Console.WriteLine($"ConnectionString: {connectionString}\r\n");

            DbProviderFactory factory = GetDbProviderFactory(provider);

            // Pull the Connection, command, reader types using the GetType() and display them in the command line 
            using (DbConnection connection = factory.CreateConnection())
            {
                Console.WriteLine($"Connection Object: {connection.GetType().Name}");

                connection.ConnectionString = connectionString;
                connection.Open();

                DbCommand command = factory.CreateCommand();
                Console.WriteLine($"Command Object: {command.GetType().Name}");

                command.Connection = connection;

                command.CommandText = "Select i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId";
                using (DbDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine($"Data reader object: {reader.GetType().Name}");

                    // Display the the car Id and Car name from the database, make it show up in command line
                    Console.WriteLine("\n\t *** Current Inventory ***");

                    while (reader.Read())
                    {
                        Console.WriteLine($"\t-> Car # {reader["Id"]} is a {reader["Name"]}");
                    }
                }
            }


        }

        // Set up connectiojn to database
        static DbProviderFactory GetDbProviderFactory(string provider)
        {
            if (provider == "SqlServer")
            {
                return SqlClientFactory.Instance;
            }
            else return null;
        }
        static (string Provider, string ConnectionString)

            GetProviderFromConfiguation()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var providerName = configuration["ProviderName"];
            var connectionString = configuration[$"{providerName}:ConnectionString"];
            return (providerName, connectionString);
        }
    }
}

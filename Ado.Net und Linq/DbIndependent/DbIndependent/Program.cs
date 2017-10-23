using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace DbIndependent
{
    class Program
    {
        static void Main(string[] args)
        {
            var shouldUseSql = true;        // aus der Konfiguration oder ähnlichem geladen/gelesen

            string connectionString;
            DbProviderFactory factory;

            if (shouldUseSql)
            {
                connectionString = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True";
                factory = SqlClientFactory.Instance;
            }
            else if (true)
            {
                connectionString = "any Access/Oracle/IBM DB2/Whatever Connection string";
                factory = System.Data.Odbc.OdbcFactory.Instance;
            }
            else
            {
                connectionString = "any other Access/Oracle/IBM DB2/Whatever Connection string";
                factory = System.Data.OleDb.OleDbFactory.Instance;
            }


            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    Console.Write("Bitte geben Sie den Suchbegriff ein: ");
                    var searchTerm = Console.ReadLine();

                    command.CommandText = @"
Select Vorname = Firstname
    , Nachname = Lastname 
    From Employees 
    Where Firstname Like @query + '%'";

                    command.AddParamterWithValues("@query", searchTerm);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var firstname = reader["Vorname"] as string;
                            var lastname = reader["Nachname"] as string;

                            Console.WriteLine($"{firstname} {lastname}");
                        }
                    }
                }
            }
        }
    }
}

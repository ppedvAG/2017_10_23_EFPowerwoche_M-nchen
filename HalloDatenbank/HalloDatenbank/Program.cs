using System;
using System.Data.SqlClient;
using System.Globalization;

namespace HalloDatenbank
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "Select Count(*) from Employees";

                    // 1. Scalare Werte
                    var result = command.ExecuteScalar();

                    switch (result) // C#7 Pattern Matching
                    {
                        case int countEmployees when countEmployees > 10:
                            Console.WriteLine($"{countEmployees} - anscheined mehr als 10 Employees in DB.");
                            break;

                        case int countEmployees when countEmployees <= 10:
                            Console.WriteLine($"{countEmployees} Employees in DB.");
                            break;

                        case string text:
                            break;

                        case bool predicate:
                            break;

                        default:
                            break;
                    }
                }

                Console.Write("Bitte geben sie einen Vornamen ein: ");
                var query = Console.ReadLine();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * From Employees Where Firstname like @search + '%'";
                    //var parameter = new SqlParameter("@search", query);

                    //var parameter = command.CreateParameter();
                    //parameter.ParameterName = "@search";
                    //parameter.Value = query;
                    //command.Parameters.Add(parameter);

                    // nur bei SqlCommand
                    command.Parameters.AddWithValue("@search", query);

                    // 2. Datensätze laden
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var lastname = reader.GetString(1);
                            var firstname = (string)reader["FirstName"];

                            var birthDate = reader.GetDateTime(5);
                            var hiredate = (DateTime)reader["HireDate"];

                            Console.WriteLine($"{lastname, -10} | {firstname, -10} | {birthDate.ToString("dd.MM.yyyy")} | {hiredate.ToString("dd.MM.yyyy")}");
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}

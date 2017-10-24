using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DelegatesInPraxis
{
    internal delegate bool MyDelegate(Employee e);
    // Action           -> void
    // Predicate        -> bool
    // Func

    class Program
    {
        static void Main(string[] args)
        {
            var employees = GetEmployees();

            //MyDelegate del = new MyDelegate(Bedinung);
            //Func<Employee, bool> del = new Func<Employee, bool>(Bedinung);
            //var del = new Func<Employee, bool>(Bedinung);
            //Func<Employee, bool> del = Bedinung;
            //var query = Abfrage(employees, del);

            //var query = Abfrage(employees, Bedinung);

            //var query = Abfrage(employees, delegate (Employee employee)
            //{
            //    return employee.Firstname.Length > 5;
            //});

            //var query = Abfrage(employees, (Employee employee) =>
            //{
            //    return employee.Firstname.Length > 5;
            //});

            //var query = Abfrage(employees, (employee) =>
            //{
            //    return employee.Firstname.Length > 5;
            //});

            //var query = Abfrage(employees, (e) => e.Firstname.Length > 5);

            var query = Abfrage(employees, e => e.Firstname.Length > 5);
            var linq = employees.Where(e => e.Firstname.Length > 5);
            var linq2 = employees.Where(Bedinung);

            foreach (var e in query)
                Console.WriteLine($"Id: {e.Id, 3} | {e.Firstname, -10} | {e.Age, 2}");
            Console.ReadKey();
        }

        private static bool Bedinung(Employee employee)
        {
            return employee.Firstname.StartsWith("S");
        }

        private static IEnumerable<Employee> Abfrage(
            IEnumerable<Employee> employees,
            Func<Employee, bool> predicate)
        {
            var query = new List<Employee>();

            foreach (var e in employees)
                if (predicate.Invoke(e))
                    query.Add(e);

            return query;
        }

        private static IEnumerable<Employee> GetEmployees()
        {
            var connectionString = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True";

            var employees = new List<Employee>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
SELECT EmployeeId
    , Firstname
    , Lastname
    , Age = DATEDIFF(hour, Birthdate, GetDate())/8766
    FROM Employees";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            employees.Add(new Employee
                            {
                                Id = (int)reader["EmployeeId"],
                                Firstname = (string)reader["Firstname"],
                                Lastname = (string)reader["Lastname"],
                                Age = (int)reader["Age"]
                            });
                    }
                }
            }

            return employees;
        }
    }
}

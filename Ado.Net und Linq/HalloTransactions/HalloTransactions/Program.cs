using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace HalloTransactions
{
    class Program
    {
        static void Main(string[] args)
        {
            var employees = new List<Employee>();

            var connectionString = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True";
            using (DbConnection connection = SqlClientFactory.Instance.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
Select EmployeeId
    , Name = Firstname + ' ' + Lastname 
    , Birthdate
    From Employees";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                Id = (int)reader["EmployeeId"],
                                Name = (string)reader["Name"],
                                Birthdate = (DateTime)reader["Birthdate"]
                            });
                        }
                    }
                }
            }

            // eine neue Connection :D

            using (var connection = SqlClientFactory.Instance.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    var countAffectedRows = 0;

                    foreach (var e in employees)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "Update Employees Set Birthdate = @birthdate Where EmployeeId = @id";

                            command.AddParamterWithValues("@birthdate", e.Birthdate.AddYears(1));
                            command.AddParamterWithValues("@id", e.Id);

                            if (e.Id == 5)
                                continue;

                            countAffectedRows += command.ExecuteNonQuery();
                        }
                    }

                    if (countAffectedRows == employees.Count)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
        }
    }
}

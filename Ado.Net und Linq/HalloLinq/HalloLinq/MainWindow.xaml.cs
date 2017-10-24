using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HalloLinq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<IEnumerable<Employee>> GetEmployees()
        {
            var connectionString = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True";

            var employees = new List<Employee>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
SELECT EmployeeId
    , Firstname
    , Lastname
    , Age = DATEDIFF(hour, Birthdate, GetDate())/8766
    FROM Employees";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
        private async Task<IEnumerable<Order>> GetOrders()
        {
            var connectionString = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True";

            var orders = new List<Order>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
select Id = OrderID
    , EmployeeID
    , City = ShipCity
	from Orders";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            orders.Add(new Order
                            {
                                Id = (int)reader["Id"],
                                EmployeeId = (int)reader["EmployeeId"],
                                City = (string)reader["City"]
                            });
                    }
                }
            }

            return orders;
        }

        private async void LoadAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            myDataGrid.ItemsSource = await GetEmployees();
        }

        private async void Restriction_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            // Linq Syntax
            //var query = from e in employees
            //            where e.Age > 50
            //            select e;

            // Fluent Api
            var query = employees.Where(e => e.Age > 50);

            myDataGrid.ItemsSource = query;
        }

        private async void Ordering_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            // Linq Syntax
            //var query = from e in employees
            //            orderby e.Age descending, e.Firstname ascending
            //            select e;

            // FluentApi
            var query = employees
                .OrderByDescending(e => e.Age)
                .ThenBy(e => e.Firstname);

            myDataGrid.ItemsSource = query;
        }

        private async void Projection_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            // Linq Syntax
            //var query = from e in employees
            //            select new 
            //            {
            //                e.Firstname,
            //                Nachname = e.Lastname
            //            };

            // Fluent API
            var query = employees.Select(e => new { Vorname = e.Firstname, Nachname = e.Lastname, Age = e.Age * 3 });

            myDataGrid.ItemsSource = query;
        }
        private class QueryClass
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
        }

        private async void Grouping_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            // Linq Syntax
            //var query = from e in employees
            //            group e by e.Age into g
            //            select new
            //            {
            //                Age = g.Key,
            //                Employees = g
            //            };

            // Fluent API
            var query = employees
                .GroupBy(e => e.Age)
                .Select(g => new { Age = g.Key, Employees = g });

            myDataGrid.ItemsSource = query.ToList();
        }

        private async void Partitioning_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            var query = employees.OrderBy(e => e.Age).Skip(3).Take(3);

            myDataGrid.ItemsSource = query.ToList();
        }

        private async void ElementOperators_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            firstEmployee.Text = employees.First().Firstname;
            firstEmployee.Text = employees.First(e => e.Age > 70).Firstname;
            firstEmployee.Text = employees.FirstOrDefault().Firstname;
            firstEmployee.Text = employees.FirstOrDefault(e => e.Age > 70).Firstname;

            //firstEmployee.Text = employees.Single().Firstname;
            //firstEmployee.Text = employees.Single(e => e.Age > 70).Firstname;
            //firstEmployee.Text = employees.SingleOrDefault().Firstname;
            //firstEmployee.Text = employees.SingleOrDefault(e => e.Age > 70).Firstname;

            firstEmployee.Text = employees.Last().Firstname;
            firstEmployee.Text = employees.Last(e => e.Age > 70).Firstname;
            firstEmployee.Text = employees.LastOrDefault().Firstname;
            firstEmployee.Text = employees.LastOrDefault(e => e.Age > 70).Firstname;
        }

        private async void Quantifying_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            var alleEmployeesInPension = employees.All(e => e.Age > 65);
            var einEmployeeInPension = employees.Any(e => e.Age > 65);
        }

        private async void Aggregating_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();

            var count = employees.Count();
            count = employees.Count(e => e.Age > 50);

            int min = employees.Min(e => e.Age);
            int max = employees.Max(e => e.Age);
            var sum = employees.Sum(e => e.Age);
            var avg = employees.Average(e => e.Age);
        }

        private async void InnerJoin_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();
            var orders = await GetOrders();

            // Linq Syntax
            //var query = from e in employees
            //            join o in orders on e.Id equals o.EmployeeId
            //            select new { e.Firstname, o.City };

            // FluentAPI
            var query = employees.Join(
                inner: orders,
                outerKeySelector: e => e.Id,
                innerKeySelector: o => o.EmployeeId,
                resultSelector: (e, o) => new { e.Firstname, o.City });

            myDataGrid.ItemsSource = query;
        }

        private async void GroupJoin_Click(object sender, RoutedEventArgs eargs)
        {
            var employees = await GetEmployees();
            var orders = await GetOrders();

            // Linq Syntax
            //var query = from e in employees
            //            join o in orders on e.Id equals o.EmployeeId into g
            //            select new { e.Firstname, Orders = g.Count() };

            // FluentAPI
            var query = employees.GroupJoin(
                inner: orders,
                outerKeySelector: e => e.Id,
                innerKeySelector: o => o.EmployeeId,
                resultSelector: (e, g) => new { e.Firstname, Orders = g.Count() });

            myDataGrid.ItemsSource = query;
        }
    }
}

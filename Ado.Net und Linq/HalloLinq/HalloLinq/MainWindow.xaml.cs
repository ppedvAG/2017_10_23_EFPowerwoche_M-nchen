using System.Collections.Generic;
using System.Data.SqlClient;
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

        private async void LoadAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            myDataGrid.ItemsSource = await GetEmployees();
        }
    }
}

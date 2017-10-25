using System;
using System.Linq;

namespace HalloEntityDataModel
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var model = new NORTHWNDEntities())
            {
                var employees = model.Employees.ToList();
                //var firstEmployee = employees.First();
                //firstEmployee.FirstName = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

                var custOrderHistory = model.CustomerOrderHistory("ANTON");

                foreach (var custOH in custOrderHistory)
                    Console.WriteLine($"{custOH.ProductName} | {custOH.Total}");

                foreach (var e in employees)
                    Console.WriteLine($"{e.Id,3} | {e.FirstName,-10} | {e.LastName}");

                //model.SaveChanges();
            }

            Console.ReadKey();
        }
    }
}

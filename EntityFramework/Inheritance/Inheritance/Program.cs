using Inheritance.Models;

namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            TPH();
        }

        private static void TPH()
        {
            using (var context = new InheritanceContext())
            {
                var c = new Customer { Name = "Sepp", ShippingAddress = "München" };
                var e = new Employee { Name = "Luis", Salary = 25000 };

                context.People.Add(c);
                context.People.Add(e);

                context.SaveChanges();
            }
        }
    }
}

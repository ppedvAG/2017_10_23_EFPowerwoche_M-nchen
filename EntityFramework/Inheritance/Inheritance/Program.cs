using Inheritance.Models;

namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            TPT();
        }

        private static void TPT()
        {
            using (var context = new InheritanceContext())
            {
                var pkw = new PKW { Geschwindigkeit = 50, Sitzplätze = 5 };
                var lkw = new LKW { Geschwindigkeit = 30, MaxLadung = 18000 };

                context.Fahrzeuge.Add(pkw);
                context.Fahrzeuge.Add(lkw);

                context.SaveChanges();
            }
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

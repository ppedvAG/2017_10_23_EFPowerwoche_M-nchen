using Inheritance.Models;
using System.Data.Entity;

namespace Inheritance
{
    public class InheritanceContext : DbContext
    {
        public InheritanceContext()
            : base("Data Source=.;Initial Catalog=InheritanceDb;Integrated Security=True")
        { }

        // TPH - Table per Hierachy
        public DbSet<Person> People { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        // TPT - Table per Type
        public DbSet<Fahrzeug> Fahrzeuge { get; set; }
        public DbSet<PKW> Pkws { get; set; }
        public DbSet<LKW> Lkws { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // TPH
            modelBuilder.Entity<Person>()
                .Map<Customer>(m => m.Requires("PersonType").HasValue("C"))
                .Map<Employee>(m => m.Requires("PersonType").HasValue("E"));

            // TPT
            modelBuilder.Entity<Fahrzeug>().ToTable("Fahrzeuge");
            modelBuilder.Entity<PKW>().ToTable("Pkws");
            modelBuilder.Entity<LKW>().ToTable("Lkws");
        }
    }
}

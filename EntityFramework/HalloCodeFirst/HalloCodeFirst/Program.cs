using HalloCodeFirst.Models;
using System;
using System.Data.Entity;
using System.Linq;
using Tynamix.ObjectFiller;

namespace HalloCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            EagerLoading();

            Console.WriteLine("Console done...");
            Console.ReadKey();
        }

        private static void EagerLoading()
        {
            using (var context = new LostStarsDbContext())
            { 
                //var galaxies = context.Galaxies.Include(g => "Stars").Take(20);  
                
                // wichtig: using System.Data.Entity;
                var galaxies = context.Galaxies.Include(g => g.Stars).Take(20);                       

                foreach (var galaxy in galaxies)                                
                {                                                               
                    Console.WriteLine($"{galaxy.Name} | {galaxy.Form}");                                 

                    foreach (var s in galaxy.Stars)
                        Console.WriteLine($"\t{s.Name} - ist {s.DistanceToEarth} Lichtjahre entfernt.");
                }
            }
        }

        private static void LazyLoading()
        {
            using (var context = new LostStarsDbContext())
            {
                // Voraussetzungen für Lazy Loading
                // - Model Klassen Public
                // - Navigationsproperties virtual
                // - context.Configuration.LazyLoadingEnabled = true;   // kann auch im Konstruktor passieren
                context.Configuration.LazyLoadingEnabled = true;        // Default: true

                var galaxies = context.Galaxies.Take(20);                       // 
                                                                                // //
                foreach (var galaxy in galaxies)                                // // // -> N + 1 Problem
                {                                                               // //
                    Console.WriteLine($"{galaxy.Name} | {galaxy.Form}");        // 

                    foreach (var s in galaxy.Stars)
                        Console.WriteLine($"\t{s.Name} - ist {s.DistanceToEarth} Lichtjahre entfernt.");
                }
            }
        }

        private static async void CreateSampleData()
        {
            var galaxyFiller = new Filler<Galaxy>();
            galaxyFiller.Setup()
                .OnProperty(g => g.Id).IgnoreIt()
                .OnProperty(g => g.Stars).IgnoreIt()
                .OnProperty(g => g.Name).Use(new RealNames(NameStyle.FirstName))
                .OnProperty(g => g.DiscoveryDate).Use(new DateTimeRange(DateTime.Now.AddYears(-300), DateTime.Now))
                .OnProperty(g => g.Description).Use(new Lipsum(LipsumFlavor.InDerFremde));

            var starFiller = new Filler<Star>();
            starFiller.Setup()
                .OnProperty(s => s.Id).IgnoreIt()
                .OnProperty(s => s.Galaxy).IgnoreIt()
                .OnProperty(s => s.GalaxyId).IgnoreIt()
                .OnProperty(s => s.Name).Use(new RealNames(NameStyle.FirstName))
                .OnProperty(s => s.DiscoveryDate).Use(new DateTimeRange(DateTime.Now.AddYears(-300), DateTime.Now))
                .OnProperty(s => s.Mass).Use(new DoubleRange(1000.0, 1000_000_000.0))
                .OnProperty(s => s.DistanceToEarth).Use(new FloatRange(0.1f, 1000_000_000.0f))
                .OnProperty(s => s.Description).Use(new Lipsum(LipsumFlavor.InDerFremde));

            var galaxies = galaxyFiller.Create(100);
            var random = new Random();

            foreach (var g in galaxies)
            {
                var stars = starFiller.Create(random.Next(10, 150));
                foreach (var s in stars)
                    g.Stars.Add(s);
            }

            using (var context = new LostStarsDbContext())
            {
                context.Galaxies.AddRange(galaxies);
                await context.SaveChangesAsync();
                Console.WriteLine("Save complete...");
            }
        }
    }
}
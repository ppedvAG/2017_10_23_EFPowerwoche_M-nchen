﻿using HalloCodeFirst.Models;
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
            Concurrency();

            Console.WriteLine("Console done...");
            Console.ReadKey();
        }

        private static void Concurrency()
        {
            using (var firstContext = new LostStarsDbContext())
            using (var secondContext = new LostStarsDbContext())
            {

                var firstGalaxyFromFirstContext = firstContext.Galaxies.First();
                var firstGalaxyFromSecondContext = secondContext.Galaxies.First();


                Console.WriteLine($"Galaxy from FirstContext: {firstGalaxyFromFirstContext.Name}");
                Console.WriteLine($"Galaxy from SecondContext: {firstGalaxyFromSecondContext.Name}");
                Console.WriteLine($"Are the same Object: {Equals(firstGalaxyFromFirstContext, firstGalaxyFromSecondContext)}");

                var aGalaxyNameHopefullyNobodyProvides = "0fe5VsvHkib7sWcL46Pc5KqIHjASTce8M6VIpucXEObtAQTmsGQBhSKxp2j8llDs5wT378XVzc9AxRDf";
                firstGalaxyFromFirstContext.Name = aGalaxyNameHopefullyNobodyProvides;
                Save(firstContext);

                Console.Write("How do you want to rename the galaxy? ");
                var newGalaxyName = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(newGalaxyName))
                {
                    Console.WriteLine("Please provide a galaxy name!");
                    newGalaxyName = Console.ReadLine();
                }

                firstGalaxyFromSecondContext.Name = newGalaxyName;

                Save(secondContext);
            }
        }
        private static void Save(LostStarsDbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    var dbValues = entry.GetDatabaseValues();
                    var modifiedNames = entry.CurrentValues.PropertyNames.Where(n => entry.Property(n).IsModified);

                    Console.WriteLine($"\nA {entry.Entity.GetType().Name} was edited in the Database.");
                    foreach (var modifiedName in modifiedNames.Where(n => n.ToLower() != "timestamp"))
                    {
                        Console.WriteLine($"Original Value for Property {modifiedName}: {entry.OriginalValues.GetValue<object>(modifiedName)}");
                        Console.WriteLine($"Your     Value for Property {modifiedName}: {entry.CurrentValues.GetValue<object>(modifiedName)}");
                        Console.WriteLine($"Database Value for Property {modifiedName}: {dbValues.GetValue<object>(modifiedName)}");
                    }

                    var wasInputInvalid = true;
                    while (wasInputInvalid)
                    {
                        wasInputInvalid = false;
                        Console.WriteLine("\nWhich Values would you like to keep?");
                        Console.WriteLine(" (1) Original Values");
                        Console.WriteLine(" (2) Your Values");
                        Console.WriteLine(" (3) Database Values");
                        var input = Console.ReadLine();
                        switch (input)
                        {
                            case "1":
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.OriginalValues.SetValues(dbValues);
                                break;
                            case "2":
                                entry.OriginalValues.SetValues(dbValues);
                                break;
                            case "3":
                                entry.Reload();
                                break;

                            default:
                                Console.WriteLine("Invalid Input");
                                wasInputInvalid = true;
                                break;
                        }
                    }
                }
                Save(context);
            }
        }

        private static void AsNoTracking()
        {
            using (var context = new LostStarsDbContext())
            {
                var stars = context.Stars.AsNoTracking().Take(10).ToList();

                Console.WriteLine($"{context.ChangeTracker.Entries().Count()} Entries in Changetracker");

                foreach (var s in stars)
                    Console.WriteLine(s.Name);

                var firstStar = stars[0];

                firstStar.Name = "The new name";
                context.Stars.Attach(firstStar);

                Console.WriteLine($"{context.ChangeTracker.Entries().Count()} Entries in Changetracker");
                Console.WriteLine(context.Entry(firstStar).State);
                //context.Entry(firstStar).State = EntityState.Modified;
                context.Entry(firstStar).Property(s => s.Name).IsModified = true;
            }
        }
        private static void PerformanceAsNoTracking()
        {
            var totalWithTracking = 0l;
            var totalAsNoTracking = 0l;
            const int iterations = 100;

            for (int i = 0; i < iterations; i++)
            {
                using (var context = new LostStarsDbContext())
                {
                    context.Galaxies.First();
                    using (new Stopwatch(t => totalWithTracking += t))
                        context.Stars.ToList();
                }

                using (var context = new LostStarsDbContext())
                {
                    context.Galaxies.First();
                    using (new Stopwatch(t => totalAsNoTracking += t))
                        context.Stars.AsNoTracking().ToList();
                }
                Console.WriteLine($"{i + 1}/{iterations}");
            }

            using (var context = new LostStarsDbContext())
            {
                var countStars = context.Stars.Count();
                Console.WriteLine($"{countStars} Stars");
            }
            Console.WriteLine($"{iterations} * WithTracking took {totalWithTracking}ms");
            Console.WriteLine($" - average {(totalWithTracking / (double)iterations):0.00}ms");
            Console.WriteLine($"{iterations} * AsNoTracking took {totalAsNoTracking}ms");
            Console.WriteLine($" - average {(totalAsNoTracking / (double)iterations):0.00}ms");
        }
        private static void ChangeTracker()
        {
            using (var context = new LostStarsDbContext())
            {
                context.Stars.First(); // Initialize Query

                context.Database.Log = Console.WriteLine;

                var galaxies = context.Galaxies.Take(10).ToList();

                var firstGalaxy = galaxies[0];
                var secondGalaxy = galaxies[1];

                firstGalaxy.Name = "Franx Xaver Gruber";

                context.Galaxies.Add(new Galaxy());
                context.Galaxies.Remove(secondGalaxy);

                Console.WriteLine($"{context.ChangeTracker.Entries<Galaxy>().Count()} Entries in ChangeTracker");

                foreach (var entry in context.ChangeTracker.Entries<Galaxy>())
                    Console.WriteLine(entry.State);

                Console.WriteLine("The modified once:");
                foreach (var entry in context.ChangeTracker.Entries<Galaxy>().Where(e => e.State == EntityState.Modified))
                {
                    foreach (var p in entry.OriginalValues.PropertyNames.Where(n => n != "Description"))
                    {
                        Console.WriteLine($"\tCurrent  {p,-13}: {entry.CurrentValues.GetValue<object>(p)}");
                        Console.WriteLine($"\tOriginal {p,-13}: {entry.OriginalValues.GetValue<object>(p)}");
                    }
                }

                //context.SaveChanges();
            }
        }
        private static void PerformanceCount()
        {
            var totalCount = 0l;
            var totalToListCount = 0l;

            var iterations = 100.0;

            for (int i = 0; i < iterations; i++)
            {
                using (var context = new LostStarsDbContext())
                {
                    context.Galaxies.First();
                    using (new Stopwatch(ms => totalCount += ms))
                        context.Stars.Count();

                    using (new Stopwatch(ms => totalToListCount += ms))
                        context.Stars.ToList().Count();
                }
                Console.WriteLine($"{i}/{iterations}");
            }

            using (var context = new LostStarsDbContext())
            {
                var countStars = context.Stars.Count();
                Console.WriteLine($"{countStars} Stars");
            }

            Console.WriteLine($"{iterations} * Count() took {totalCount}ms");
            Console.WriteLine($" - average {(totalCount / iterations):0.00}ms");
            Console.WriteLine($"{iterations} * ToList().Count() took {totalToListCount}ms");
            Console.WriteLine($" - average {(totalToListCount / iterations):0.00}ms");
            // !!! NEVER  use ToList().Count or ToList().Count() just to count !!!
        }
        private static void IQueryable()
        {
            using (var context = new LostStarsDbContext())
            {
                context.Database.Log = Console.WriteLine;

                var galaxyQuery = context.Galaxies.Where(g => g.DiscoveryDate.Year > 1900);
                galaxyQuery = galaxyQuery.OrderBy(g => g.Name);
                galaxyQuery = galaxyQuery.Where(g => g.DiscoveryDate.Year < 1950);
                //galaxyQuery = galaxyQuery.Where(g => g.Stars.Count() < 30);

                var galaxies = galaxyQuery.ToList();
                foreach (var g in galaxies)
                    Console.WriteLine($"{g.Name,-11} | {g.Form,-10} | {g.DiscoveryDate.Year}");
            }
        }
        private static void PreLoading()
        {
            using (var context = new LostStarsDbContext())
            {
                context.Stars.Where(s => s.GalaxyId == 1).ToList();
                // 100 Stars werden im context gecached.

                // weil die Stars für diese Galaxy bereits im context gecached sind,
                // werden die Navigationsproperties automatisch richtig initialisiert
                // ! nur bei 1-N Verbindungen !
                // genannt: Relationship Fixup
                var galaxy = context.Galaxies.Single(g => g.Id == 1);

                Console.WriteLine($"{galaxy.Name} | {galaxy.Form}");
                foreach (var s in galaxy.Stars)
                    Console.WriteLine($"\t{s.Name} - ist {s.DistanceToEarth} Lichtjahre entfernt.");
            }
        }
        private static void EagerLoading()
        {
            using (var context = new LostStarsDbContext())
            {
                //var galaxies = context.Galaxies.Include("Stars").Take(20);  

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
            // http://objectfiller.net/
            // https://objectfillernet.readme.io/v1.5.3/reference
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
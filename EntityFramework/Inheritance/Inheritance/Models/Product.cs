using System;

namespace Inheritance.Models
{
    public abstract class Product   // abstract bei TPC zwingend
    {
        public Guid Id { get; set; }
        public string Material { get; set; }
    }
}

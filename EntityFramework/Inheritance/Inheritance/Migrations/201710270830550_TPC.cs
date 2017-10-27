namespace Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TPC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tische",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Material = c.String(),
                        AnzahlBeine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uhren",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Material = c.String(),
                        Zeit = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Uhren");
            DropTable("dbo.Tische");
        }
    }
}

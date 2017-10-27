namespace Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fahrzeuge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Geschwindigkeit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lkws",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MaxLadung = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fahrzeuge", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Pkws",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Sitzplätze = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fahrzeuge", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pkws", "Id", "dbo.Fahrzeuge");
            DropForeignKey("dbo.Lkws", "Id", "dbo.Fahrzeuge");
            DropIndex("dbo.Pkws", new[] { "Id" });
            DropIndex("dbo.Lkws", new[] { "Id" });
            DropTable("dbo.Pkws");
            DropTable("dbo.Lkws");
            DropTable("dbo.Fahrzeuge");
        }
    }
}

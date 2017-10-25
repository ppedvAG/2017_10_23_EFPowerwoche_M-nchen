namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStarsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DiscoveryDate = c.DateTime(nullable: false),
                        Mass = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistanceToEarth = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stars");
        }
    }
}

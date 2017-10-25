namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigureGalaxyStarsRelationShip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stars_Table", "GalaxyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stars_Table", "GalaxyId");
            AddForeignKey("dbo.Stars_Table", "GalaxyId", "dbo.Galaxies_Table", "GalaxyId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stars_Table", "GalaxyId", "dbo.Galaxies_Table");
            DropIndex("dbo.Stars_Table", new[] { "GalaxyId" });
            DropColumn("dbo.Stars_Table", "GalaxyId");
        }
    }
}

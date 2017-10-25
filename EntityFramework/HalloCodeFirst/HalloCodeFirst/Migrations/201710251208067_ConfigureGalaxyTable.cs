namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigureGalaxyTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Galaxies", newName: "Galaxies_Table");
            RenameColumn(table: "dbo.Galaxies_Table", name: "Id", newName: "GalaxyId");
            RenameColumn(table: "dbo.Galaxies_Table", name: "Form", newName: "GalaxyForm");
            AlterColumn("dbo.Galaxies_Table", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Galaxies_Table", "DiscoveryDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Galaxies_Table", "DiscoveryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Galaxies_Table", "Name", c => c.String());
            RenameColumn(table: "dbo.Galaxies_Table", name: "GalaxyForm", newName: "Form");
            RenameColumn(table: "dbo.Galaxies_Table", name: "GalaxyId", newName: "Id");
            RenameTable(name: "dbo.Galaxies_Table", newName: "Galaxies");
        }
    }
}
namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigureStarsTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Stars", newName: "Stars_Table");
            RenameColumn(table: "dbo.Stars_Table", name: "Id", newName: "StarId");
            AlterColumn("dbo.Stars_Table", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Stars_Table", "DiscoveryDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stars_Table", "DiscoveryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Stars_Table", "Name", c => c.String());
            RenameColumn(table: "dbo.Stars_Table", name: "StarId", newName: "Id");
            RenameTable(name: "dbo.Stars_Table", newName: "Stars");
        }
    }
}

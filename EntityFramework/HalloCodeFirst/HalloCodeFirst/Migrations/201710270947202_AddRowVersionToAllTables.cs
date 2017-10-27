namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRowVersionToAllTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galaxies_Table", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Stars_Table", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stars_Table", "Timestamp");
            DropColumn("dbo.Galaxies_Table", "Timestamp");
        }
    }
}

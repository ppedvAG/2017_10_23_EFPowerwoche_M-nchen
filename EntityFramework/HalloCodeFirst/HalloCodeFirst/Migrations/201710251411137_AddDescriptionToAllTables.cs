namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToAllTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galaxies_Table", "Description", c => c.String());
            AddColumn("dbo.Stars_Table", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stars_Table", "Description");
            DropColumn("dbo.Galaxies_Table", "Description");
        }
    }
}

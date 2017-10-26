namespace HalloCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDefaultStringConvention : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Galaxies_Table", "Name", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Stars_Table", "Name", c => c.String(nullable: false, maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stars_Table", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Galaxies_Table", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}

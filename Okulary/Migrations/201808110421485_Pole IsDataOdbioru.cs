namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoleIsDataOdbioru : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Binocles", "IsDataOdbioru", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Binocles", "IsDataOdbioru");
        }
    }
}

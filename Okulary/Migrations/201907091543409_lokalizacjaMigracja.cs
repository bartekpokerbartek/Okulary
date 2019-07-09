namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lokalizacjaMigracja : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Lokalizacja", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Lokalizacja");
        }
    }
}

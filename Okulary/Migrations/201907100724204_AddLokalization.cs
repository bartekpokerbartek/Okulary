namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLokalization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Elements", "Lokalizacja", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Elements", "Lokalizacja");
        }
    }
}

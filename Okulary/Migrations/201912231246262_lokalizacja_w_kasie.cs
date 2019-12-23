namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lokalizacja_w_kasie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MoneyCounts", "Lokalizacja", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MoneyCounts", "Lokalizacja");
        }
    }
}

namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinoclesExtendedFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Binocles", "RodzajOprawekBliz", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Binocles", "RodzajOprawekBliz", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

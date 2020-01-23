namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WyplatyIBugFixPlatnosciKartaKwota : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payouts",
                c => new
                    {
                        PayoutId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        Lokalizacja = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PayoutId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Payouts");
        }
    }
}

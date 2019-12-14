namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MonyCuntaded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyCounts",
                c => new
                    {
                        MoneyCountId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MoneyCountId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MoneyCounts");
        }
    }
}

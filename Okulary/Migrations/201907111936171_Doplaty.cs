namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Doplaty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doplatas",
                c => new
                    {
                        DoplataId = c.Int(nullable: false, identity: true),
                        Kwota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDoplaty = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DoplataId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Doplatas");
        }
    }
}

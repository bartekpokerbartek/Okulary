namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ElementsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        ElementId = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(),
                        Ilosc = c.Int(nullable: false),
                        Cena = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataSprzedazy = c.DateTime(nullable: false),
                        DataUtworzenia = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ElementId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Elements");
        }
    }
}

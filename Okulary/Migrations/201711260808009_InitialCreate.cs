namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Binocles",
                c => new
                    {
                        BinocleId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        BuyDate = c.DateTime(nullable: false),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.BinocleId)
                .ForeignKey("dbo.People", t => t.Person_PersonId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Binocles", "Person_PersonId", "dbo.People");
            DropIndex("dbo.Binocles", new[] { "Person_PersonId" });
            DropTable("dbo.People");
            DropTable("dbo.Binocles");
        }
    }
}

namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestBin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Binocles", "Person_PersonId1", "dbo.People");
            DropIndex("dbo.Binocles", new[] { "Person_PersonId1" });
            DropColumn("dbo.Binocles", "Person_PersonId1");
            //RenameColumn(table: "dbo.Binocles", name: "Person_PersonId1", newName: "Person_PersonId");
            AlterColumn("dbo.Binocles", "Person_PersonId", c => c.Int(nullable: false));
            CreateIndex("dbo.Binocles", "Person_PersonId");
            AddForeignKey("dbo.Binocles", "Person_PersonId", "dbo.People", "PersonId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Binocles", "Person_PersonId", "dbo.People");
            DropIndex("dbo.Binocles", new[] { "Person_PersonId" });
            AlterColumn("dbo.Binocles", "Person_PersonId", c => c.Int());
            //RenameColumn(table: "dbo.Binocles", name: "Person_PersonId", newName: "Person_PersonId1");
            AddColumn("dbo.Binocles", "Person_PersonId1", c => c.Int(nullable: false));
            CreateIndex("dbo.Binocles", "Person_PersonId1");
            AddForeignKey("dbo.Binocles", "Person_PersonId1", "dbo.People", "PersonId");
        }
    }
}

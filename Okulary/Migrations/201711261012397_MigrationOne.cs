namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationOne : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Binocles", "Person_PersonId", "dbo.People");
            DropIndex("dbo.Binocles", new[] { "Person_PersonId" });
            AddColumn("dbo.Binocles", "Person_PersonId1", c => c.Int());
            AlterColumn("dbo.Binocles", "Person_PersonId", c => c.Int(nullable: false));
            CreateIndex("dbo.Binocles", "Person_PersonId1");
            AddForeignKey("dbo.Binocles", "Person_PersonId1", "dbo.People", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Binocles", "Person_PersonId1", "dbo.People");
            DropIndex("dbo.Binocles", new[] { "Person_PersonId1" });
            AlterColumn("dbo.Binocles", "Person_PersonId", c => c.Int());
            DropColumn("dbo.Binocles", "Person_PersonId1");
            CreateIndex("dbo.Binocles", "Person_PersonId");
            AddForeignKey("dbo.Binocles", "Person_PersonId", "dbo.People", "PersonId");
        }
    }
}

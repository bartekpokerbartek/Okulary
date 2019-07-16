namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracja : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Doplatas", "Binocle_BinocleId", "dbo.Binocles");
            DropIndex("dbo.Doplatas", new[] { "Binocle_BinocleId" });
            AddColumn("dbo.Doplatas", "Binocle_BinocleId1", c => c.Int());
            AlterColumn("dbo.Doplatas", "Binocle_BinocleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Doplatas", "Binocle_BinocleId1");
            AddForeignKey("dbo.Doplatas", "Binocle_BinocleId1", "dbo.Binocles", "BinocleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doplatas", "Binocle_BinocleId1", "dbo.Binocles");
            DropIndex("dbo.Doplatas", new[] { "Binocle_BinocleId1" });
            AlterColumn("dbo.Doplatas", "Binocle_BinocleId", c => c.Int());
            DropColumn("dbo.Doplatas", "Binocle_BinocleId1");
            CreateIndex("dbo.Doplatas", "Binocle_BinocleId");
            AddForeignKey("dbo.Doplatas", "Binocle_BinocleId", "dbo.Binocles", "BinocleId");
        }
    }
}

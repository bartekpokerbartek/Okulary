namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoplataFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Doplatas", "Binocle_BinocleId1", "dbo.Binocles");
            DropIndex("dbo.Doplatas", new[] { "Binocle_BinocleId1" });
            DropColumn("dbo.Doplatas", "Binocle_BinocleId1");
            //RenameColumn(table: "dbo.Doplatas", name: "Binocle_BinocleId1", newName: "Binocle_BinocleId");
            AlterColumn("dbo.Doplatas", "Binocle_BinocleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Doplatas", "Binocle_BinocleId");
            AddForeignKey("dbo.Doplatas", "Binocle_BinocleId", "dbo.Binocles", "BinocleId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doplatas", "Binocle_BinocleId", "dbo.Binocles");
            DropIndex("dbo.Doplatas", new[] { "Binocle_BinocleId" });
            AlterColumn("dbo.Doplatas", "Binocle_BinocleId", c => c.Int());
            //RenameColumn(table: "dbo.Doplatas", name: "Binocle_BinocleId", newName: "Binocle_BinocleId1");
            AddColumn("dbo.Doplatas", "Binocle_BinocleId1", c => c.Int(nullable: false));
            CreateIndex("dbo.Doplatas", "Binocle_BinocleId1");
            AddForeignKey("dbo.Doplatas", "Binocle_BinocleId1", "dbo.Binocles", "BinocleId");
        }
    }
}

namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBinocle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doplatas", "Binocle_BinocleId", c => c.Int());
            CreateIndex("dbo.Doplatas", "Binocle_BinocleId");
            AddForeignKey("dbo.Doplatas", "Binocle_BinocleId", "dbo.Binocles", "BinocleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doplatas", "Binocle_BinocleId", "dbo.Binocles");
            DropIndex("dbo.Doplatas", new[] { "Binocle_BinocleId" });
            DropColumn("dbo.Doplatas", "Binocle_BinocleId");
        }
    }
}

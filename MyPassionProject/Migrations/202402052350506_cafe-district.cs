namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cafedistrict : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cafes", "DistrictId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cafes", "DistrictId");
            AddForeignKey("dbo.Cafes", "DistrictId", "dbo.Districts", "DistrictId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cafes", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Cafes", new[] { "DistrictId" });
            DropColumn("dbo.Cafes", "DistrictId");
        }
    }
}

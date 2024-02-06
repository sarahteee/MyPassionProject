namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cafeamenities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Amenities",
                c => new
                    {
                        AmenityId = c.Int(nullable: false, identity: true),
                        AmenityName = c.String(),
                    })
                .PrimaryKey(t => t.AmenityId);
            
            CreateTable(
                "dbo.AmenityCafes",
                c => new
                    {
                        Amenity_AmenityId = c.Int(nullable: false),
                        Cafe_CafeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Amenity_AmenityId, t.Cafe_CafeId })
                .ForeignKey("dbo.Amenities", t => t.Amenity_AmenityId, cascadeDelete: true)
                .ForeignKey("dbo.Cafes", t => t.Cafe_CafeId, cascadeDelete: true)
                .Index(t => t.Amenity_AmenityId)
                .Index(t => t.Cafe_CafeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AmenityCafes", "Cafe_CafeId", "dbo.Cafes");
            DropForeignKey("dbo.AmenityCafes", "Amenity_AmenityId", "dbo.Amenities");
            DropIndex("dbo.AmenityCafes", new[] { "Cafe_CafeId" });
            DropIndex("dbo.AmenityCafes", new[] { "Amenity_AmenityId" });
            DropTable("dbo.AmenityCafes");
            DropTable("dbo.Amenities");
        }
    }
}

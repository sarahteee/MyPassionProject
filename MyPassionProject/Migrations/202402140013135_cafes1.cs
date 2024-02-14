namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cafes1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AmenityCafes", newName: "CafeAmenities");
            DropPrimaryKey("dbo.CafeAmenities");
            AddPrimaryKey("dbo.CafeAmenities", new[] { "Cafe_CafeId", "Amenity_AmenityId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CafeAmenities");
            AddPrimaryKey("dbo.CafeAmenities", new[] { "Amenity_AmenityId", "Cafe_CafeId" });
            RenameTable(name: "dbo.CafeAmenities", newName: "AmenityCafes");
        }
    }
}

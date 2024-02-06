namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cafes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cafes",
                c => new
                    {
                        CafeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Description = c.String(),
                        Website = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.CafeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cafes");
        }
    }
}

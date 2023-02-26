namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rentals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rentals",
                c => new
                    {
                        RentalID = c.Int(nullable: false, identity: true),
                        RFName = c.String(),
                        RLName = c.String(),
                        PurchaseDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        MovieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RentalID)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .Index(t => t.MovieID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rentals", "MovieID", "dbo.Movies");
            DropIndex("dbo.Rentals", new[] { "MovieID" });
            DropTable("dbo.Rentals");
        }
    }
}

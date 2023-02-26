namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        RentalID = c.Int(nullable: false, identity: true),
                        RFName = c.String(),
                        RLName = c.String(),
                        PurchaseDate = c.Int(nullable: false),
                        ReturnDate = c.Int(nullable: false),
                        MovieID = c.String(),
                    })
                .PrimaryKey(t => t.RentalID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Movies");
        }
    }
}

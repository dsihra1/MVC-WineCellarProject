namespace username_Wines.DAL.WineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wine_Type",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        wtName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Wine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        wineName = c.String(nullable: false, maxLength: 70),
                        wineYear = c.String(nullable: false, maxLength: 4),
                        winePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        wineHarvest = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => new { t.wineName, t.wineYear }, unique: true, name: "IX_Unique_Wine");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Wine", "IX_Unique_Wine");
            DropTable("dbo.Wine");
            DropTable("dbo.Wine_Type");
        }
    }
}

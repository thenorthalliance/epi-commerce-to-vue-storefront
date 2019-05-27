using System.Data.Entity.Migrations;

namespace EPiServer.Vsf.DataAccess.Migrations
{
    public partial class AddRefreshTokenStore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRefreshTokens",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetRefreshTokens", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRefreshTokens", new[] { "UserId" });
            DropTable("dbo.AspNetRefreshTokens");
        }
    }
}

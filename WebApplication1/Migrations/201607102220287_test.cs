namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyUserClaims",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        MyUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MyUsers", t => t.MyUser_Id)
                .Index(t => t.MyUser_Id);
            
            CreateTable(
                "dbo.MyUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyUserClaims", "MyUser_Id", "dbo.MyUsers");
            DropIndex("dbo.MyUserClaims", new[] { "MyUser_Id" });
            DropTable("dbo.MyUsers");
            DropTable("dbo.MyUserClaims");
        }
    }
}

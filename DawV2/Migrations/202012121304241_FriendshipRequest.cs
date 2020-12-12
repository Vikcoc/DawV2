namespace DawV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendshipRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendshipRequests",
                c => new
                    {
                        FriendshipRequestId = c.Int(nullable: false, identity: true),
                        RequesterId = c.String(maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        Message = c.String(),
                        IsSeen = c.Boolean(nullable: false),
                        IsAccepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FriendshipRequestId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .ForeignKey("dbo.AspNetUsers", t => t.RequesterId)
                .Index(t => t.RequesterId)
                .Index(t => t.ReceiverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendshipRequests", "RequesterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendshipRequests", "ReceiverId", "dbo.AspNetUsers");
            DropIndex("dbo.FriendshipRequests", new[] { "ReceiverId" });
            DropIndex("dbo.FriendshipRequests", new[] { "RequesterId" });
            DropTable("dbo.FriendshipRequests");
        }
    }
}

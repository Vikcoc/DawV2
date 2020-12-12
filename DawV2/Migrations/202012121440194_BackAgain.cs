namespace DawV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BackAgain : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FriendshipRequests", "RequesterId", "dbo.AspNetUsers");
            DropIndex("dbo.FriendshipRequests", new[] { "RequesterId" });
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        Content = c.String(nullable: false),
                        IdEdited = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.GroupMessages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        Message = c.String(),
                        IsEdited = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                        Description = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUserId, t.GroupId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        NoticeId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.NoticeId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        Content = c.String(nullable: false),
                        IsEdited = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            AlterColumn("dbo.FriendshipRequests", "RequesterId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.FriendshipRequests", "RequesterId");
            AddForeignKey("dbo.FriendshipRequests", "RequesterId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendshipRequests", "RequesterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Comments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notices", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupMessages", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupMessages", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.FriendshipRequests", new[] { "RequesterId" });
            DropIndex("dbo.Posts", new[] { "ApplicationUserId" });
            DropIndex("dbo.Notices", new[] { "ApplicationUserId" });
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "ApplicationUserId" });
            DropIndex("dbo.GroupMessages", new[] { "ApplicationUserId" });
            DropIndex("dbo.GroupMessages", new[] { "GroupId" });
            DropIndex("dbo.Comments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            AlterColumn("dbo.FriendshipRequests", "RequesterId", c => c.String(maxLength: 128));
            DropTable("dbo.Posts");
            DropTable("dbo.Notices");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMessages");
            DropTable("dbo.Comments");
            CreateIndex("dbo.FriendshipRequests", "RequesterId");
            AddForeignKey("dbo.FriendshipRequests", "RequesterId", "dbo.AspNetUsers", "Id");
        }
    }
}

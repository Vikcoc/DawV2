﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DawV2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public bool IsPublic { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
        public virtual ICollection<Notice> Notices { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FriendshipRequest> Requested { get; set; }
        public virtual ICollection<FriendshipRequest> Received { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FriendshipRequest> FriendshipRequests { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupMessage> GroupMessages { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>()
                .HasOptional(x => x.ApplicationUser)
                .WithMany(x => x.Comments)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<FriendshipRequest>()
                .HasOptional(x => x.Receiver)
                .WithMany(x => x.Received)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<FriendshipRequest>()
                .HasRequired(x => x.Requester)
                .WithMany(x => x.Requested);
                //.WillCascadeOnDelete(true);
        }
    }
}
using DawV2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DawV2.Startup))]
namespace DawV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);


                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";

                var adminCreated = UserManager.Create(user, "Admin*1");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}

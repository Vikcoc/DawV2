using DawV2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DawV2.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Search()
        {
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        public ActionResult Search(ApplicationUser usr)
        {
            bool isAdmin = User.IsInRole("Admin");
            var users = from user in db.Users
                        where user.LastName == usr.LastName && user.FirstName == usr.FirstName && (user.IsPublic == true || isAdmin == true)
                        select user;

            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            ViewBag.UsersList = users;
            ViewBag.NrUtilizatori = users.Count();

            return View();
        }

        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;
            ViewBag.UsersList = users;
            return View();
        }

        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            string currentRole = user.Roles.FirstOrDefault().RoleId;

            var userRoleName = (from role in db.Roles
                                where role.Id == currentRole
                                select role.Name).First();

            ViewBag.roleName = userRoleName;

            return View(user);
        }

       
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }

        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            try
            {
                if (TryUpdateModel(user))
                {
                    user.FirstName = newData.FirstName;
                    user.LastName = newData.LastName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {

            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);

            var posts = db.Posts.Where(a => a.ApplicationUserId == id);
            foreach (var post in posts)
            {
                db.Posts.Remove(post);

            }

            var comments = db.Comments.Where(comm => comm.ApplicationUserId == id);
            foreach (var comment in comments)
            {
                db.Comments.Remove(comment);
            }

            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }
    }
}
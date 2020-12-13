using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DawV2.Models;
using Microsoft.AspNet.Identity;

namespace DawV2.Controllers
{
    [Authorize/*(Roles = "Admin")*/]
    public class NoticeController : Controller
    {
        // GET: Notice
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var id = User.Identity.GetUserId();
            ViewBag.Notices = await _db.Notices.Where(x => x.ApplicationUserId == id).ToListAsync();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult New(string id)
        {
            //var user = _db.Users.Find(userId);
            //if (user != null)
            //    ViewBag.UserId = userId;
            var notice = new Notice
            {
                ApplicationUserId = id
            };
            return View(notice);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(Notice notice)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(notice);
                _db.Notices.Add(notice);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(notice);
            }
        }
    }
}
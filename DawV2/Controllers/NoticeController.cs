using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DawV2.Models;

namespace DawV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NoticeController : Controller
    {
        // GET: Notice
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(string userId)
        {
            var user = _db.Users.Find(userId);
            if (user != null)
                ViewBag.UserId = userId;
            return View(new Notice{ApplicationUserId = userId});
        }

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
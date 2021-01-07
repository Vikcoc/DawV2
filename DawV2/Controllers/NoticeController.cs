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
    [Authorize]
    public class NoticeController : Controller
    {
        // GET: Notice
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var id = User.Identity.GetUserId();
            ViewBag.Notices = await _db.Notices.Where(x => x.ApplicationUserId == id && x.Seen == false).ToListAsync();
            return View();
        }

        public async Task<ActionResult> OkIt(int id)
        {
            var notice = await _db.Notices.FindAsync(id);
            var userId = User.Identity.GetUserId();
            if (notice?.ApplicationUserId != userId)
                return Redirect(Request.UrlReferrer.ToString());
            notice.Seen = true;
            await _db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
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
                if (Request.UrlReferrer != null) 
                    return Redirect(Request.UrlReferrer.ToString());
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(notice);
            }
        }
    }
}
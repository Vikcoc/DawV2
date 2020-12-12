using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DawV2.Models;
using Microsoft.AspNet.Identity;

namespace DawV2.Controllers
{
    [Authorize]
    public class FriendshipRequestsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: FriendshipRequests
        public int Index()
        {
            var num = User.Identity.GetUserId();
            return _db.FriendshipRequests.Count(x => x.ReceiverId == num);
        }

        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(FriendshipRequest request)
        {
            request.RequesterId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    _db.FriendshipRequests.Add(request);
                    _db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(request);
                }
            }
            catch (Exception)
            {
                return View(request);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DawV2.Models;
using Microsoft.AspNet.Identity;

namespace DawV2.Controllers
{
    [Authorize]
    public class FriendshipRequestController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: FriendshipRequests
        public ActionResult Index()
        {
            var num = User.Identity.GetUserId();
            ViewBag.Requests = _db.FriendshipRequests.Where(x => x.IsSeen == false && x.ReceiverId == num).Include(x => x.Requester);
            //return _db.FriendshipRequests.Count(x => x.RequesterId == num);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            return View();
        }

        public ActionResult Send(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Send(string id, FriendshipRequest request)
        {

            request.RequesterId = User.Identity.GetUserId();
            request.ReceiverId = id;

            if (_db.FriendshipRequests.FirstOrDefault(x =>
                x.RequesterId == request.RequesterId && x.ReceiverId == request.ReceiverId) != null)
            {
                TempData["message"] = "Request already sent";
                return RedirectToAction("Index");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _db.FriendshipRequests.Add(request);
                    _db.SaveChanges();
                    TempData["message"] = "Cererea a fost adaugata";
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
        
        public ActionResult Accept(int id)
        {
            try
            {
                var oldRequest = _db.FriendshipRequests.Find(id);
                if(TryUpdateModel(oldRequest))
                {
                    oldRequest.IsAccepted = true;
                    _db.SaveChanges();
                }

                return RedirectToAction("Index");

            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                var request = _db.FriendshipRequests.Find(id);
                _db.FriendshipRequests.Remove(request);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
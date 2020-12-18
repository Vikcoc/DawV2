using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using DawV2.Models;
using Microsoft.AspNet.Identity;

namespace DawV2.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        // GET: Group
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var userId = User.Identity.GetUserId();
            ViewBag.Groups = _context.Groups.Include(x => x.UserGroups/*.Where(y => y.ApplicationUserId == userId)*/).ToList();
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Group group)
        {
            if(group.UserGroups == null)
                group.UserGroups = new List<UserGroup>
                {
                    new UserGroup
                    {
                        ApplicationUserId = User.Identity.GetUserId()
                    }
                };
            try
            {
                if (!ModelState.IsValid)
                    return View(group);
                @group.CreationDate = DateTime.UtcNow;
                _context.Groups.Add(@group);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(group);
            }
        }

        public ActionResult Show(int id)
        {
            var group = _context.Groups.Include(x => x.GroupMessages).Include(x => x.UserGroups).FirstOrDefault(x => x.GroupId == id);
            if (group == null)
                return RedirectToAction("Index");
            return View(group);
        }

        public ActionResult Edit(int id)
        {
            var group = _context.Groups.Find(id);
            return View(group);
        }

        [HttpPut]
        public ActionResult Edit(Group group)
        {
            var userId = User.Identity.GetUserId();
            if (_context.UserGroups.Find(userId, group.GroupId) == null)
                return RedirectToAction("Show", new { id = group.GroupId });
            try
            {
                if (!ModelState.IsValid)
                    return View(group);
                var dbGroup = _context.Groups.Find(group.GroupId);

                if (!TryUpdateModel(dbGroup))
                    return View(group);

                if (dbGroup == null)
                    return RedirectToAction("Show", new {id = @group.GroupId});
                dbGroup.GroupName = @group.GroupName;
                dbGroup.Description = @group.Description;
                _context.SaveChanges();

                return RedirectToAction("Show", new { id = group.GroupId });
            }
            catch (Exception)
            {
                return RedirectToAction("Show", new { id = group.GroupId });
            }

        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            if (_context.UserGroups.Find(userId, id) == null)
            {
                TempData["message"] = "Nu esti parte din grup";
                return RedirectToAction("Index");
            }
            var group = _context.Groups.Find(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                TempData["message"] = "Grupul cu numele: " + group.GroupName + " a fost sters";
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Adhere(int id)
        {
            var userId = User.Identity.GetUserId();
            if (_context.UserGroups.Find(userId, id) != null) 
                return RedirectToAction("Index");
            var group = _context.Groups.Find(id);
            if(@group == null)
                return RedirectToAction("Index");
            _context.UserGroups.Add(new UserGroup
            {
                GroupId = id,
                ApplicationUserId = userId
            });
            _context.SaveChanges();
            TempData["message"] = "Ai intrat in grupul cu numele: " + @group.GroupName;
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Leave(int id)
        {
            var userId = User.Identity.GetUserId();
            var thing = _context.UserGroups.Find(userId, id);
            if (thing == null)
                return RedirectToAction("Index");
            _context.UserGroups.Remove(thing);
            _context.SaveChanges();
            var group = _context.Groups.Find(id);
            TempData["message"] = "Ai parasit grupul cu numele: " + group.GroupName;
            return RedirectToAction("Index");
        }
    }
}
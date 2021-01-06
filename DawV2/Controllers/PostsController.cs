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
    [Authorize(Roles = "User,Admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.posts = _db.Posts.Include(x => x.ApplicationUser);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }
            return View();
        }

        public ActionResult Show(int id)
        {
            Post post = _db.Posts.Find(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }
            return View(post);
        }
        
        [Authorize(Roles = "User,Admin")]
        public ActionResult New()
        {
            Post post = new Post();
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Post post)
        {
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            
            post.IsEdited = false;
            post.ApplicationUserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Posts.Add(post);
                    _db.SaveChanges();
                    TempData["message"] = "Postarea a fost adaugata cu succes!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(post);
                }
            }
            catch (Exception)
            {
                return View(post);
            }

        }
        
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            Post post = _db.Posts.Find(id);
            return View(post);
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (post.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        Post oldPost = _db.Posts.Find(id);
                        if (TryUpdateModel(oldPost))
                        {
                            oldPost.Content = post.Content;
                            _db.SaveChanges();
                            TempData["message"] = "Postarea a fost editata cu succes";
                        }
                        return Redirect("/Posts/Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreprul sa modificati aceasta postare";
                        return Redirect("/Posts/Index");
                    }
                }
                else
                {
                    return View(post);
                }
            }
            catch (Exception)
            {
                return View(post);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            var post = _db.Posts.Find(id);
            if (post.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                _db.Posts.Remove(post);
                _db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa cu succes!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti aceasta postare!";
                return RedirectToAction("Index");
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
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
        // GET: Posts
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.posts = _db.Posts;
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }
            return View();
        }

        public ActionResult Show(int id)
        {
            Post post = _db.Posts.Find(id);
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }
            return View(post);
        }

        public ActionResult New()
        {
            Post post = new Post();
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            return View(post);
        }

        [HttpPost]
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

        public ActionResult Edit(int id)
        {
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            Post post = _db.Posts.Find(id);
            return View(post);
        }

        [HttpPut]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                if (ModelState.IsValid)
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
                    return View(post);
                }

            }
            catch (Exception)
            {
                return View(post);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var post = _db.Posts.Find(id);
            _db.Posts.Remove(post);
            _db.SaveChanges();
            TempData["message"] = "Postarea a fost stearsa cu succes!";
            return RedirectToAction("Index");
        }
    }
}
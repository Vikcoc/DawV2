using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DawV2.Models;
using Microsoft.AspNet.Identity;

namespace DawV2.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Comment comment)
        {
            comment.ApplicationUserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Comments.Add(comment);
                    _db.SaveChanges();
                    return Redirect("/Posts/Show/" + comment.PostId);
                }
                else
                {
                    return Redirect("/Posts/Show/" + comment.PostId);
                }

            }
            catch (Exception)
            {
                return Redirect("/Posts/Show/" + comment.PostId);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            
            var comment = _db.Comments.Find(id);
            if (comment.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
                return Redirect("/Posts/Show/" + comment.PostId);
            }
            else
            {
                return Redirect("/Posts/Show/" + comment.PostId);
            }
        }
        
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            Comment comment = _db.Comments.Find(id);
            return View(comment);
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                if (comment.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    var oldComment = _db.Comments.Find(id);
                    if (TryUpdateModel(oldComment))
                    {
                        oldComment.Content = comment.Content;
                        _db.SaveChanges();
                    }

                    return Redirect("/Posts/Show/" + comment.PostId);
                }
                else
                {
                    return Redirect("/Posts/Show/" + comment.PostId);
                }
            }
            catch (Exception)
            {
                return Redirect("/Posts/Show/" + comment.PostId);
            }
        }
    }
}
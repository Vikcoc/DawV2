using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DawV2.Models;

namespace DawV2.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();


        [HttpPost]
        public ActionResult New(Comment comment)
        {
            comment.ApplicationUserId = _db.Users.First().Id;
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
        public ActionResult Delete(int id)
        {
            var comment = _db.Comments.Find(id);
            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return Redirect("/Posts/Show/" + comment.PostId);
        }

        public ActionResult Edit(int id)
        {
            Comment comment = _db.Comments.Find(id);
            return View(comment);
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                var oldComment = _db.Comments.Find(id);
                if (TryUpdateModel(oldComment))
                {
                    oldComment.Content = comment.Content;
                    _db.SaveChanges();
                }

                return Redirect("/Posts/Show/" + comment.PostId);
            }
            catch (Exception)
            {
                return Redirect("/Posts/Show/" + comment.PostId);
            }
        }
    }
}
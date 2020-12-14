using System;
using System.Web.Mvc;
using DawV2.Models;
using Microsoft.AspNet.Identity;

namespace DawV2.Controllers
{
    public class GroupMessageController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();


        [HttpPost]
        public ActionResult New(GroupMessage message)
        {
            message.ApplicationUserId = User.Identity.GetUserId();
            try
            {
                if (!ModelState.IsValid) 
                    return Redirect("/Groups/Show/" + message.GroupId);
                
                _db.GroupMessages.Add(message);
                _db.SaveChanges();

                return Redirect("/Groups/Show/" + message.GroupId);

            }
            catch (Exception)
            {
                return Redirect("/Groups/Show/" + message.GroupId);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var comment = _db.Comments.Find(id);
            if (comment == null)
                return Redirect("/Groups");

            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return Redirect("/Groups/Show/" + comment.PostId);
        }

        public ActionResult Edit(int id)
        {
            var groupMessage = _db.GroupMessages.Find(id);
            return View(groupMessage);
        }

        [HttpPut]
        public ActionResult Edit(int id, GroupMessage groupMessage)
        {
            try
            {
                var oldMessage = _db.Comments.Find(id);
                if (!TryUpdateModel(oldMessage) || oldMessage == null)
                    return Redirect("/Groups/Show/" + groupMessage.GroupId);

                oldMessage.Content = groupMessage.Message;
                _db.SaveChanges();

                return Redirect("/Groups/Show/" + groupMessage.GroupId);
            }
            catch (Exception)
            {
                return Redirect("/Groups/Show/" + groupMessage.GroupId);
            }
        }
    }
}
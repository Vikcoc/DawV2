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
        public ActionResult New(GroupMessage groupMessage)
        {
            groupMessage.ApplicationUserId = User.Identity.GetUserId();
            try
            {
                if (!ModelState.IsValid) 
                    return Redirect("/Groups/Show/" + groupMessage.GroupId);
                
                _db.GroupMessages.Add(groupMessage);
                _db.SaveChanges();

                return Redirect("/Groups/Show/" + groupMessage.GroupId);

            }
            catch (Exception)
            {
                return Redirect("/Groups/Show/" + groupMessage.GroupId);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var message = _db.GroupMessages.Find(id);
            if (message == null)
                return Redirect("/Groups");

            _db.GroupMessages.Remove(message);
            _db.SaveChanges();
            return Redirect("/Groups/Show/" + message.GroupId);
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
                var oldMessage = _db.GroupMessages.Find(id);
                if (!TryUpdateModel(oldMessage) || oldMessage == null)
                    return Redirect("/Groups/Show/" + groupMessage.GroupId);

                oldMessage.Message = groupMessage.Message;
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
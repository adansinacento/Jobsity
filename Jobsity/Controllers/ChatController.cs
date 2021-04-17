using Jobsity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jobsity.Controllers
{
    public class ChatController : Controller
    {
        private jobsityEntities db = new jobsityEntities();

        [HttpPost]
        public ActionResult NewMessage(string msg)
        {
            //verify sender user
            var senderName = Session["user"].ToString();
            var sender = db.Users.Where(u => u.Username == senderName).FirstOrDefault();

            //if it comes to this point means user session expired or something
            if (sender == null)
            {
                return RedirectToAction("Login", "Users");
            }

            //create msg obj
            Message newMessage = new Message() { 
                Message1 = msg, 
                Sender = sender.Id, 
                TimeSent = DateTime.Now
            };

            //add to db
            db.Messages.Add(newMessage);
            db.SaveChanges();

            return PartialView("~/Views/Shared/_MessagePartial.cshtml", newMessage);
        }

        public ActionResult Room()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            //get latest 50 messages
            var list = db.Messages.OrderBy(m => m.TimeSent).Take(50);
            return View(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
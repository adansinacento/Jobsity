using Jobsity.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobsity.Helpers;
using Jobsity.Helpers.Extensions;
using System.Threading.Tasks;

namespace Jobsity.Controllers
{
    public class ChatController : Controller
    {
        private jobsityEntities db = new jobsityEntities();

        [HttpPost]
        public void NewMessage(string msg)
        {
            //verify sender user
            var senderName = Session["user"].ToString();
            var sender = db.Users.Where(u => u.Username == senderName).FirstOrDefault();

            //if it comes to this point means user session expired or something
            if (sender == null)
            {
                return;
            }

            //create msg obj
            Message newMessage = new Message() { 
                Message1 = msg, 
                Sender = sender.Id, 
                TimeSent = DateTime.Now
            };

            //send to all clients
            var context = GlobalHost.ConnectionManager.GetHubContext<chatHub>();
            context.Clients.All.message(senderName, msg, newMessage.TimeSent.ToString("d/MMM HH:mm:ss"));

            //check it was a command for the bot
            const string stockBotCommandInit = "/stock=";
            if (msg.StartsWith(stockBotCommandInit))
            {
                //get only the string after /stock=
                var stockCode = msg.RemoveFirstInstance(stockBotCommandInit);
                
                //send to api and get response
                StockBot bot = new StockBot(stockCode);
                var stockVal = bot.GetStock();

                var header = "🤖 Stock bot";
                var footer = DateTime.Now.ToString("d/MMM HH:mm:ss") + " | quote requested by " + senderName;
                
                if (!stockVal.Success)
                {
                    stockVal.Content = "Error: " + stockVal.Content;
                }

                //send message to clients
                context.Clients.All.message(header, stockVal.Content, footer);
            }
            else
            {
                //add to db (only if i was not a command for the bot)
                db.Messages.Add(newMessage);
                db.SaveChanges();
            }
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
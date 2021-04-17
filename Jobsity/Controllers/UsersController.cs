using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jobsity.Models;
using BC = BCrypt.Net;

namespace Jobsity.Controllers
{
    public class UsersController : Controller
    {
        private jobsityEntities db = new jobsityEntities();

        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName, string Password)
        {
            var user = db.Users.Where(u => u.Username == UserName).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (BC.BCrypt.Verify(Password, user.PassHash))
            {
                Session["user"] = UserName;
                return RedirectToAction("Room", "Chat");
            }

            
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string UserName, string PassHash)
        {
            //first check username is not taken
            if (db.Users.Any(u => u.Username == UserName))
            {
                return RedirectToAction("Register");
            }

            //if username is available register the new user

            //actually password is not hashed here so lets hash it
            var salt = BC.BCrypt.GenerateSalt();
            var hash = BC.BCrypt.HashPassword(PassHash, salt);

            var newUser = new User()
            {
                Username = UserName, 
                PassHash = hash, 
                RegistrationTime = DateTime.Now
            };

            try
            {
                db.Users.Add(newUser);
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }

            //User is created now lets log them in
            return null;
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

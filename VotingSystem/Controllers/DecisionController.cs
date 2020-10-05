using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VotingSystem.Models;
using WebMatrix.WebData;

namespace VotingSystem.Controllers
{
    public class DecisionController : Controller
    {
        VotingSystemEntities db = new VotingSystemEntities(); 
      
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome, this page is available for the authorized users only";
            if (Session["LoggedInUserID"] == null)
            {
              return  LogoffUser();
            }
            return View(db.Decisions.ToList());
        }
        
        [Authorize]
        public ActionResult Create()
        {
            if (Session["LoggedInUserID"] == null)
            {
              return  LogoffUser(); 
            }
            int UID = int.Parse(Session["LoggedInUserID"].ToString());
            bool isAdmin = (bool)db.Users.Find(UID).IsAdmin;
            if (!isAdmin)
            {
                return RedirectToAction("Index");
            }
            return View(); 
        }
                     
        [HttpPost]
        public ActionResult Create(Decision decision)
        {
            if (ModelState.IsValid)
            {
                db.Decisions.Add(decision);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(decision); 
        }
        
        [ChildActionOnly]
        public ActionResult LogoffUser()
        {

            return RedirectToAction("SignOut", "Account");
        }

    }
}

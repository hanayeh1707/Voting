using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class VoteController : Controller
    {
        private VotingSystemEntities db = new VotingSystemEntities();

        [Authorize]
        public ActionResult Vote(int decisionID, int userID)
        {
            Vote decisionVote = db.Votes.Where(V => V.UserID == userID && V.DecisionID == decisionID).FirstOrDefault();
            if (decisionVote == null)
            {
                decisionVote = new Vote();
                decisionVote.DecisionID = decisionID;
                decisionVote.UserID = userID;
               
            }
            return View(decisionVote);
        }

        [HttpPost]
        public ActionResult Vote(Vote vote)
        {
            if (ModelState.IsValid)
            {
             Vote decisionVote = db.Votes.Where(V=>V.DecisionID== vote.DecisionID && V.UserID==vote.UserID).FirstOrDefault(); 
             if (decisionVote == null)// in case of new vote 
             {
                 db.Votes.Add(vote);
             }
             else // in case of editing the vote
             {
                 decisionVote.Vote1 = vote.Vote1;
                 db.Votes.Attach(decisionVote);
                 db.Entry(decisionVote).State = EntityState.Modified;
                 //db.Entry(decisionVote).CurrentValues.SetValues(vote);
             }
                db.SaveChanges();
                
                int AgreeVotesNo = getAgreeVotesCount((int)vote.DecisionID);
               
                int DisagreeVotesNo = getDisagreeVotesCount((int)vote.DecisionID); 

                return Json(new VotingResultModel{ AgreeVotes = AgreeVotesNo, DisagreeVotes = DisagreeVotesNo});
            }
            
            ViewBag.ErrorMessage = "Something went wrong, please try again";
            
            return View(vote);
        }
 
        [ChildActionOnly]
        public int getAgreeVotesCount(int DecisionID)
        {
            return db.Votes.Where(V => V.DecisionID == DecisionID && V.Vote1 == 1).Count();
        }
        
        [ChildActionOnly]
        public int getDisagreeVotesCount(int DecisionID)
        {
            return db.Votes.Where(V => V.DecisionID == DecisionID && V.Vote1 == 2).Count();
        }
  
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
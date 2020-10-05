using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class VotingResultModel
    {
        public int AgreeVotes
        {
            get;
            set;
        }
        public int DisagreeVotes
        {
            get;
            set;
        }
    }
}
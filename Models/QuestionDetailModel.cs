using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.Models
{
    public class QuestionDetailModel
    {
        public int QuestionNo { get; set; }
        public string SuveryID { get; set; }
        public string DetailTitle { get; set; }
        public string DetailType { get; set; }
        public string DetailMustKeyin { get; set; }
        public string ItemName { get; set; }

    }
}
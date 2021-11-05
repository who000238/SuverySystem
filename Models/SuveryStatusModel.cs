using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.Models
{
    /// <summary> this model is used for check suvery status</summary>
    public class SuveryStatusModel //just build  temporary not use
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime TodayDate { get; set; }
        public string NowStatus { get; set; }
    }
}
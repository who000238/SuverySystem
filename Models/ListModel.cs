using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.Models
{
    public class ListModel
    {
        public int No { get; set; }
        public string SuveryID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string ClassName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SDate
        {
            get { return StartDate.ToString("yyyy-MM-dd"); }
        }
        public string EDate
        {
            get { return EndDate.ToString("yyyy-MM-dd"); }
        }
    }
}
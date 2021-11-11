using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.Models
{
    public class SuveryMasterModel
    {
        public Guid SuveryNo { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime  EndDate{ get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }

    }
}
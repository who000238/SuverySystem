using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.Models
{
    public class UserInfoModel
    {
        public int No { get; set; }
        public string SuveryID { get; set; }
        public string UserInfoName { get; set; }
        public string UserInfoString { get; set; }

        public string CreateTimeString { get; set; }
    }
}
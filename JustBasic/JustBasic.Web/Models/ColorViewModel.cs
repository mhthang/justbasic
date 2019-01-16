using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class ColorViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Image { set; get; }
        public string Description { set; get; }
        public bool Status { set; get; }
    }
}
using JustBasic.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class MemmberViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}
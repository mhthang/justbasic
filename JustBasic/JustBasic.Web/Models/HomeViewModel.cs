using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductCategoryViewModel> listProductCategory { set; get; }
        public IEnumerable<SlideViewModel> Slides { set; get; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}
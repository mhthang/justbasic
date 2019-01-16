using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }

        public int ID { set; get; }

        public int ProductID { set; get; }

        public int Quantitty { set; get; }

        public decimal Price { set; get; }
        public decimal? Discount { set; get; }
    }
}
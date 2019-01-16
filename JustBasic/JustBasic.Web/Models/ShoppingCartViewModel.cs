using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int Id { set; get; }
        public int ProductId { set; get; }
        public ProductViewModel Product { set; get; }
        public int SizeId { set; get; }
        public int ColorId { set; get; }
        public string ColorName { set; get; }
        public string SizeName { set; get; }
        public int Quantity { set; get; }
        public decimal SalePrice { set; get; }
        public decimal? PromotionPrice { set; get; }
    }
}
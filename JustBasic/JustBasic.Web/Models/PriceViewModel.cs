using JustBasic.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class PriceViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn sản phẩm")]
        public int ProductID { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn size")]
        public int SizeID { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn màu")]
        public int ColorID { set; get; }

        public virtual Product Product { set; get; }

        public virtual Size Size { set; get; }

        public virtual Color Color { set; get; }
        [Required(ErrorMessage = "Yêu cầu nhập giá")]
        public decimal SalePrice { set; get; }

        public decimal? PromotionPrice { set; get; }

        public bool IsPrimary { set; get; }
    }
}
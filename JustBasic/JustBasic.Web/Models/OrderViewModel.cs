using JustBasic.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class OrderViewModel
    {
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { set; get; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { set; get; }

        public string Description { set; get; }

        [MaxLength(256)]
        public string PaymentMethod { set; get; }

        public decimal TotalAmount { set; get; }
        public decimal? TotalDiscount { set; get; }

        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public string PaymentStatus { set; get; }
        public bool Status { set; get; }

        [MaxLength(128)]
        public string CustomerId { set; get; }

        public string BankCode { set; get; }
        public ApplicationUser User { set; get; }
        public IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}
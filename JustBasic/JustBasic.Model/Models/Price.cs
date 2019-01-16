using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustBasic.Model.Models
{
    [Table("Prices")]
    public class Price
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public int ProductID { set; get; }
        public int SizeID { set; get; }

        public int ColorID { set; get; }

        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }

        [ForeignKey("SizeID")]
        public virtual Size Size { set; get; }

        [ForeignKey("ColorID")]
        public virtual Color Color { set; get; }

        public decimal SalePrice { set; get; }
        public decimal? PromotionPrice { set; get; }

        public bool IsPrimary { set; get; }
    }
}
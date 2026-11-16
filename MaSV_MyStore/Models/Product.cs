using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSV_MyStore.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int? Quantity { get; set; }

        public int? SoldQuantity { get; set; } = 0;

        [StringLength(250)]
        public string ImageUrl { get; set; }

        public int CategoryID { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSV_MyStore.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(50)]
        public string DeliveryMethod { get; set; }

        [StringLength(250)]
        public string ShippingAddress { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

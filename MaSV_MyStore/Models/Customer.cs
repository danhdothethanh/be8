using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSV_MyStore.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Username")]
        public virtual User User { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSV_MyStore.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Role { get; set; } = "Customer";

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public virtual Customer Customer { get; set; }
    }
}

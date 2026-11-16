using System.ComponentModel.DataAnnotations;

namespace MaSV_MyStore.Models.ViewModel
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        [StringLength(250)]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức giao hàng")]
        [Display(Name = "Phương thức giao hàng")]
        public string DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }

        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

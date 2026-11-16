namespace MaSV_MyStore.Models.ViewModel
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}

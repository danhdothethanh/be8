using System.Collections.Generic;
using MaSV_MyStore.Models;

namespace MaSV_MyStore.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
        public List<Product> TopProducts { get; set; }
        public int Quantity { get; set; } = 1;
    }
}

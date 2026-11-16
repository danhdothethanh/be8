using System.Collections.Generic;
using MaSV_MyStore.Models;
using PagedList;

namespace MaSV_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        public List<Product> FeaturedProducts { get; set; }
        public IPagedList<Product> NewProducts { get; set; }
        public string SearchTerm { get; set; }
    }
}

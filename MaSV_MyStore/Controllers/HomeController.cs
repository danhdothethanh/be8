using System.Linq;
using System.Web.Mvc;
using MaSV_MyStore.Models;
using MaSV_MyStore.Models.ViewModel;
using PagedList;

namespace MaSV_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Home/Index
        public ActionResult Index(string searchTerm = "", int page = 1)
        {
            int pageSize = 6;
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) 
                    || (p.Description != null && p.Description.Contains(searchTerm)));
            }

            var model = new HomeProductVM
            {
                FeaturedProducts = db.Products.OrderByDescending(p => p.SoldQuantity ?? 0).Take(10).ToList(),
                NewProducts = products.OrderByDescending(p => p.CreatedDate).ToPagedList(page, pageSize),
                SearchTerm = searchTerm
            };

            return View(model);
        }

        // GET: Home/ProductList
        public ActionResult ProductList(int id, int page = 1)
        {
            int pageSize = 9;
            var category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();

            var products = db.Products.Where(p => p.CategoryID == id)
                .OrderByDescending(p => p.CreatedDate)
                .ToPagedList(page, pageSize);

            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryID = id;
            return View(products);
        }

        // GET: Home/ProductDetail
        public ActionResult ProductDetail(int id, int page = 1)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            var model = new ProductDetailVM
            {
                Product = product,
                RelatedProducts = db.Products.Where(p => p.CategoryID == product.CategoryID && p.ProductID != id)
                    .Take(8).ToList(),
                TopProducts = db.Products.Where(p => p.CategoryID == product.CategoryID)
                    .OrderByDescending(p => p.SoldQuantity ?? 0).Take(8).ToList()
            };

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}

using System.Linq;
using System.Web.Mvc;
using MaSV_MyStore.Models;
using MaSV_MyStore.Models.ViewModel;

namespace MaSV_MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        private CartService cartService = new CartService();

        public ActionResult Index()
        {
            var cart = cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product == null) return HttpNotFound();

            var cart = cartService.GetCart();
            cart.AddItem(new CartItem
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ImageUrl = product.ImageUrl,
                UnitPrice = product.Price,
                Quantity = quantity,
                Category = product.Category?.CategoryName
            });
            cartService.SaveCart(cart);

            TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            var cart = cartService.GetCart();
            cart.RemoveItem(productId);
            cartService.SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = cartService.GetCart();
            cart.UpdateQuantity(productId, quantity);
            cartService.SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            var cart = cartService.GetCart();
            cart.Clear();
            cartService.SaveCart(cart);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}

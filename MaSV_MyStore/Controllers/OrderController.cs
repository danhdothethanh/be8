using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MaSV_MyStore.Models;
using MaSV_MyStore.Models.ViewModel;

namespace MaSV_MyStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        private CartService cartService = new CartService();

        // GET: Order/Checkout
        public ActionResult Checkout()
        {
            var cart = cartService.GetCart();
            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.";
                return RedirectToAction("Index", "Home");
            }

            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Order") });
            }

            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);

            var model = new CheckoutVM
            {
                ShippingAddress = customer?.Address ?? "",
                FullName = customer?.FullName ?? "",
                Phone = customer?.Phone ?? "",
                Email = customer?.Email ?? "",
                TotalAmount = cart.TotalAmount,
                DeliveryMethod = "Standard",
                PaymentMethod = "COD"
            };

            ViewBag.Cart = cart;
            return View(model);
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = cartService.GetCart();
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                model.TotalAmount = cart.TotalAmount;
                ViewBag.Cart = cart;
                return View(model);
            }

            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tạo Order
            var order = new Order
            {
                Username = user.Username,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalAmount,
                PaymentStatus = "Pending",
                PaymentMethod = model.PaymentMethod,
                DeliveryMethod = model.DeliveryMethod,
                ShippingAddress = model.ShippingAddress,
                Status = "Pending"
            };

            db.Orders.Add(order);
            db.SaveChanges(); // Lấy OrderID

            // Tạo OrderDetails
            foreach (var item in cart.Items)
            {
                var detail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                db.OrderDetails.Add(detail);

                // Cập nhật số lượng đã bán
                var product = db.Products.Find(item.ProductID);
                if (product != null)
                {
                    product.SoldQuantity = (product.SoldQuantity ?? 0) + item.Quantity;
                }
            }
            db.SaveChanges();

            // Xóa giỏ hàng
            cart.Clear();
            cartService.SaveCart(cart);
            Session["Cart"] = null;
            Session["CartCount"] = 0;

            return RedirectToAction("OrderSuccess", new { id = order.OrderID });
        }

        // GET: Order/OrderSuccess
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders
                .Include(o => o.OrderDetails.Select(d => d.Product))
                .SingleOrDefault(o => o.OrderID == id && o.Username == User.Identity.Name);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Order/MyOrder  (Lịch sử mua hàng)
        public ActionResult MyOrder(string status = "All", string search = "")
        {
            var query = db.Orders
                .Where(o => o.Username == User.Identity.Name)
                .Include(o => o.OrderDetails.Select(d => d.Product))
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(o => o.Status == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    o.OrderID.ToString().Contains(search) ||
                    o.OrderDetails.Any(d => d.Product.ProductName.Contains(search)));
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();

            ViewBag.Status = status;
            ViewBag.Search = search;
            return View(orders);
        }

        // Alias theo đề (OrderHistory)
        public ActionResult OrderHistory(string status = "All", string search = "")
        {
            return RedirectToAction("MyOrder", new { status, search });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MaSV_MyStore.Models;
using MaSV_MyStore.Models.ViewModel;

namespace MaSV_MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                    return View(model);
                }

                // Tạo bản ghi User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Trong thực tế nên hash password
                    Role = "Customer",
                    CreatedDate = DateTime.Now
                };

                // Tạo bản ghi Customer
                var customer = new Customer
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = model.Address,
                    CreatedDate = DateTime.Now
                };

                db.Users.Add(user);
                db.Customers.Add(customer);
                db.SaveChanges();

                // Chuyển hướng về Trang chủ
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Lưu thông tin tài khoản vào Session
                    Session["Username"] = user.Username;
                    Session["Role"] = user.Role;

                    // Lưu thông tin xác thực vào Cookie
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/ProfileInfo
        [Authorize]
        public ActionResult ProfileInfo()
        {
            string username = User.Identity.Name;
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return RedirectToAction("Login");
            }

            var model = new RegisterVM
            {
                Username = customer.Username,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address
            };
            return View(model);
        }

        // POST: Account/UpdateProfile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                var customer = db.Customers.SingleOrDefault(c => c.Username == username);
                if (customer != null)
                {
                    customer.FullName = model.FullName;
                    customer.Phone = model.Phone;
                    customer.Email = model.Email;
                    customer.Address = model.Address;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                }
            }
            return RedirectToAction("ProfileInfo");
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            string username = User.Identity.Name;
            var user = db.Users.SingleOrDefault(u => u.Username == username);

            if (user == null || user.Password != currentPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng.";
                return RedirectToAction("ProfileInfo");
            }

            if (newPassword != confirmNewPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới và xác nhận không khớp.";
                return RedirectToAction("ProfileInfo");
            }

            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                return RedirectToAction("ProfileInfo");
            }

            user.Password = newPassword;
            db.SaveChanges();
            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ProfileInfo");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

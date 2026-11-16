# MaSV_MyStore - BE8 Customer Site (ASP.NET MVC5 + EF)

Dự án BE8 - Tiếp tục từ BE7, bổ sung đầy đủ tính năng **Đặt hàng + Xác nhận đơn hàng + Lịch sử mua hàng**.

## Tính năng BE8 (mới / nâng cấp)

### Đặt hàng (Order/Checkout)
- Kiểm tra giỏ hàng rỗng → redirect Home
- Bắt buộc đăng nhập ([Authorize])
- Lấy địa chỉ mặc định từ Customer
- Chọn phương thức giao hàng (Standard / Express)
- Chọn phương thức thanh toán (COD / BankTransfer / PayPal)
- Tạo Order + OrderDetail
- Cập nhật SoldQuantity sản phẩm
- Xóa giỏ hàng sau khi đặt thành công

### Xác nhận đơn hàng (Order/OrderSuccess)
- Hiển thị đầy đủ thông tin đơn hàng
- Chi tiết từng sản phẩm (tên, SL, đơn giá, thành tiền)

### Lịch sử mua hàng (Order/MyOrder)
- Tab lọc theo trạng thái: Tất cả / Chờ xử lý / Đang xử lý / Đang vận chuyển / Đã giao / Đã hủy
- Tìm kiếm theo mã đơn hoặc tên sản phẩm
- Xem chi tiết từng đơn

## Các tính năng từ BE7 vẫn giữ nguyên
- Customer Layout + Menu Category + Menu cá nhân
- Đăng ký / Đăng nhập / Đăng xuất / Profile
- Home + Tìm kiếm + Phân trang
- ProductList / ProductDetail
- Giỏ hàng (Cart)

## Cấu trúc Order

```
Controllers/OrderController.cs
Models/Order.cs, OrderDetail.cs
Models/ViewModel/CheckoutVM.cs
Views/Order/
  ├── Checkout.cshtml
  ├── OrderSuccess.cshtml
  └── MyOrder.cshtml
```

## Cách chạy
1. Mở MaSV_MyStore.sln bằng Visual Studio
2. Restore NuGet Packages
3. Rebuild Solution
4. F5

## Database (theo đề)
- Bảng Order: có DeliveryMethod, PaymentMethod, ShippingAddress, Status, PaymentStatus
- Bảng OrderDetail: có TotalPrice (có thể dùng computed column)


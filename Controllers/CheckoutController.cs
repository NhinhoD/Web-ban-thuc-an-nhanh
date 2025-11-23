using ASM_C_4.Areas.Admin.Repository;
using ASM_C_4.Models;
using ASM_C_4.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASM_C_4.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IEmailSender _emailSender;
		public CheckoutController(DataContext context, IEmailSender emailSender)
		{
			_dataContext = context;
			_emailSender = emailSender;
		}
        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 1. Kiểm tra giỏ hàng trước. Nếu rỗng thì đuổi về
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            if (cartItems.Count == 0)
            {
                TempData["error"] = "Giỏ hàng trống, vui lòng chọn sản phẩm!";
                return RedirectToAction("Index", "Home");
            }

            // 2. Bắt đầu Transaction (Để đảm bảo tính toàn vẹn dữ liệu)
            using var transaction = await _dataContext.Database.BeginTransactionAsync();

            try
            {
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel
                {
                    OrderCode = ordercode,
                    UserName = userEmail,
                    Status = 1,
                    CreatedDate = DateTime.UtcNow // QUAN TRỌNG: Dùng UtcNow cho PostgreSQL
                };

                // Lưu Order cha trước
                _dataContext.Add(orderItem);
                await _dataContext.SaveChangesAsync();

                foreach (var cart in cartItems)
                {
                    if (cart.IsCombo)
                    {
                        // Lấy thông tin combo từ DB để chắc chắn dữ liệu đúng
                        var combo = await _dataContext.Combos
                            .Include(c => c.ComboProducts)
                            .ThenInclude(cp => cp.Product)
                            .FirstOrDefaultAsync(c => c.Id == cart.ProductId);

                        if (combo != null)
                        {
                            foreach (var comboProduct in combo.ComboProducts)
                            {
                                var orderDetails = new OrderDetails
                                {
                                    UserName = userEmail,
                                    OrderCode = ordercode,
                                    ProductId = comboProduct.ProductId,
                                    Price = comboProduct.Product.Price,
                                    Quantity = cart.Quantity,
                                    IsCombo = true
                                };
                                _dataContext.Add(orderDetails); // Chỉ Add vào bộ nhớ, chưa lưu xuống DB
                            }
                        }
                    }
                    else // Sản phẩm thường
                    {
                        var orderDetails = new OrderDetails
                        {
                            UserName = userEmail,
                            OrderCode = ordercode,
                            ProductId = cart.ProductId,
                            Price = cart.Price,
                            Quantity = cart.Quantity,
                            IsCombo = false
                        };
                        _dataContext.Add(orderDetails); // Chỉ Add vào bộ nhớ
                    }
                }

                // 3. Lưu toàn bộ OrderDetails MỘT LẦN DUY NHẤT
                await _dataContext.SaveChangesAsync();

                // 4. Commit Transaction (Xác nhận mọi thứ thành công)
                await transaction.CommitAsync();

                // Xóa giỏ hàng
                HttpContext.Session.Remove("Cart");

                // Gửi email (Để trong try-catch riêng để nếu lỗi mail cũng không rollback đơn hàng)
                try
                {
                    var receiver = userEmail;
                    var subject = "Đặt hàng thành công";
                    var message = $"Đơn hàng của bạn đã được đặt thành công. Mã đơn hàng: {ordercode}";
                    await _emailSender.SendEmailAsync(receiver, subject, message);
                }
                catch
                {
                    // Gửi mail lỗi thì thôi, bỏ qua, không làm phiền người dùng
                }

                TempData["success"] = "Tạo đơn hàng thành công! Vui lòng đợi duyệt đơn hàng!";
                return RedirectToAction("Index", "Cart"); // Hoặc trang Lịch sử đơn hàng
            }
            catch (Exception ex)
            {
                // Nếu có lỗi gì xảy ra trong quá trình lưu, Rollback lại hết (Không tạo đơn rác)
                await transaction.RollbackAsync();
                TempData["error"] = "Có lỗi xảy ra khi xử lý đơn hàng: " + ex.Message;
                return RedirectToAction("Index", "Cart");
            }
        }


    }
}

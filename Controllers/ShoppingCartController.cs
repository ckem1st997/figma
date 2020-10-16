using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using figma.DAL;
using figma.Interface;
using figma.Models;
using figma.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace figma.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailer _mailer;
        private const string CartCookieKey = "CartID";

        public ShoppingCartController(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMailer mailer)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mailer = mailer;
            if (!_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(CartCookieKey))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieKey, Guid.NewGuid().ToString(),
                 new CookieOptions()
                 {
                     SameSite = SameSiteMode.Lax,
                     Secure = true,
                     Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                 });
            }
        }

        [Route("gio-hang/thong-tin")]
        public IActionResult Index()
        {
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = GetCartItems(),
                CartTotal = GetTotal() < 1000000 ? (GetTotal() + 30000) : GetTotal()
            };

            if (HttpContext.Request.Cookies[".AspNetCore.Cookies"] != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                if (userId != null)
                {
                    try
                    {
                        ViewBag.address = _unitOfWork.MemberRepository.GetByID(int.Parse(userId));
                    }
                    catch (Exception)
                    {
                        ViewBag.address = null;
                    }
                }
            }
            return View(viewModel);
        }
        [Route("thanh-toan")]
        public IActionResult Order()
        {
            if (!GetCartItems().Any())
            {
                return RedirectToAction("Index");
            }
            var model = new CheckOutViewModel
            {
                Order = new Order(),
                Carts = GetCartItems(),
                CartTotal = GetTotal() < 1000000 ? (GetTotal() + 30000) : GetTotal()
            };
            return View(model);
        }
        [Route("thanh-toan")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(CheckOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                //lấy ra danh sách đơn hàng
                var item = GetCartItems();
                // ngày giao hàng
                model.Order.TransportDate = DateTime.TryParse(model.Order.TransportDate.ToString(), new CultureInfo("Vi"), DateTimeStyles.None, out var tDate) ? tDate : DateTime.Now;
                _unitOfWork.OrderRepository.Insert(model.Order);
                await _unitOfWork.Save();

                model.Order.MaDonHang = DateTime.Now.ToString("yyyyMMddHHmm") + "C" + model.Order.Id;
                foreach (var odetails in from cart1 in item
                                         select new OrderDetail
                                         {
                                             OrderId = model.Order.Id,
                                             ProductId = cart1.ProductID,
                                             Quantity = cart1.Count,
                                             Price = cart1.Price,
                                             Color = cart1.Color == null ? "null" : cart1.Color,
                                             Size = cart1.Size == null ? "null" : cart1.Size
                                         })
                {
                    _unitOfWork.OrderDetailRepository.Insert(odetails);
                }
                await _unitOfWork.Save();

                var typepay = "Thanh toán khi nhận hàng";
                switch (model.TypePay)
                {
                    case 1:
                        typepay = "Tiền mặt";
                        break;
                    case 2:
                        typepay = "Chuyển khoản";
                        break;
                    case 3:
                        typepay = "Hình thức khác";
                        break;
                }

                var giaohang = "Đến địa chỉ người nhận";
                switch (model.Transport)
                {
                    case 3:
                        giaohang = "Khách đến nhận hàng";
                        break;
                    case 1:
                        giaohang = "Qua bưu điện";
                        break;
                    case 4:
                        giaohang = "Hình thức khác";
                        break;
                }
                var tongtien = 0m;
                StringBuilder sb = new StringBuilder();
                sb.Append("<p>Mã đơn hàng: <strong>" + model.Order.MaDonHang + "</strong></p>");
                sb.Append("<p>Họ và tên: <strong>" + model.Order.Fullname + "</strong></p>");
                sb.Append("<p>Địa chỉ: <strong>" + model.Order.Address + "</strong></p>");
                sb.Append("<p>Email: <strong>" + model.Order.Email + "</strong></p>");
                sb.Append("<p>Điện thoại: <strong>" + model.Order.Mobile + "</strong></p>");
                sb.Append("<p>Yêu cầu thêm: <strong>" + model.Order.Body + "</strong></p>");
                sb.Append("<p>Ngày đặt hàng: <strong>" + model.Order.CreateDate.ToString("dd-MM-yyyy HH:ss") + "</strong></p>");
                sb.Append("<p>Ngày giao hàng: <strong>" + model.Order.TransportDate.ToString("dd-MM-yyyy") + "</strong></p>");
                sb.Append("<p>Hình thức giao hàng: <strong>" + giaohang + "</strong></p>");
                sb.Append("<p>Hình thức thanh toán: <strong>" + typepay + "</strong></p>");
                sb.Append("<p>Thông tin đơn hàng</p>");
                sb.Append("<table border='1' cellpadding='10' style='border:1px #ccc solid;border-collapse: collapse'>" +
                      "<tr>" +
                      "<th>Ảnh sản phẩm</th>" +
                      "<th>Tên sản phẩm</th>" +
                      "<th>Số lượng</th>" +
                      "<th>Giá tiền</th>" +
                      "<th>Thành tiền</th>" +
                      "</tr>");
                foreach (var odetails in model.Order.OrderDetails)
                {
                    var thanhtien = Convert.ToDecimal(odetails.Price * odetails.Quantity);
                    tongtien += thanhtien;

                    var img = "NO PICTURE";
                    if (odetails.Product.Image != null)
                    {
                        img = "<img src='https://" + Request.Host.Value + "/" + odetails.Product.Image.Split(',')[0] + "' />";
                    }

                    sb.Append("<tr>" +
                          "<td>" + img + "</td>" +
                          "<td>" + "" + odetails.Product.Name + "-Color:" + odetails.Color + "-Size:" + odetails.Size + "");

                    sb.Append("</td>" +
                          "<td style='text-align:center'>" + odetails.Quantity + "</td>" +
                          "<td style='text-align:center'>" + Convert.ToDecimal(odetails.Price).ToString("N0") + "</td>" +
                          "<td style='text-align:center'>" + thanhtien.ToString("N0") + " đ</td>" +
                          "</tr>");
                }

                sb.Append("<tr><td colspan='5' style='text-align:right'><strong>Tổng tiền: " + tongtien.ToString("N0") + " đ</strong></td></tr>");
                sb.Append("</table>");
                sb.Append("<p>Cảm ơn bạn đã tin tưởng và mua hàng của chúng tôi.</p>");
                await _mailer.SendEmailSync(model.Order.Email, "[" + model.Order.MaDonHang + "] Đơn đặt hàng từ website ShopAsp.Net", sb.ToString());
                return RedirectToAction("CheckOutComplete", new { orderId = model.Order.MaDonHang });
            }
            model.Carts = GetCartItems();
            model.CartTotal = GetTotal() < 1000000 ? (GetTotal() + 30000) : GetTotal();
            return View(model);
        }

        [Route("thanh-toan-thanh-cong")]
        public ActionResult CheckOutComplete(string orderId)
        {
            EmptyCart();
            var model = new CheckOutCompleteViewModel()
            {
                OrderID = orderId,
                Contact = _unitOfWork.ContactRepository.Get().FirstOrDefault()
            };
            return View(model);
        }


        public void     EmptyCart()
        {
            var cartItems = _unitOfWork.CartRepository.Get(cart => cart.CartID == GetCartId());

            foreach (var cartItem in cartItems)
            {
                _unitOfWork.CartRepository.Delete(cartItem);
            }
            _unitOfWork.SaveNotAync();
        }

        #region CartRes
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult AddToCartAjax(int quantity, int productId, string color, string size)
        {
            decimal price = 0;
            var addedProduct = _unitOfWork.ProductRepository.Get(a => a.ProductID == productId).SingleOrDefault();
            if (addedProduct == null)
            {
                return Json(new { result = 0, count = 0 });
            }
            if (addedProduct.SaleOff > 0)
            {
                price = addedProduct.SaleOff;
            }
            else if (addedProduct.Price > 0)
            {
                price = addedProduct.Price;
            }
            var cartItem = _unitOfWork.CartRepository.Get(c => c.CartID == GetCartId() && c.ProductID == addedProduct.ProductID && c.Color == color && c.Size == size).SingleOrDefault();

            if (cartItem == null)
            {
                cartItem = new Carts
                {
                    ProductID = addedProduct.ProductID,
                    Price = price,
                    CartID = GetCartId(),
                    Count = quantity,
                    Color = color,
                    Size = size,
                    DateCreated = DateTime.Now
                };
                _unitOfWork.CartRepository.Insert(cartItem);
            }
            else
            {
                cartItem.Count += quantity;
            }
            _unitOfWork.SaveNotAync();
            var data = new
            {
                result = 1,
                count = GetCount()
            };
            return Json(new { data });
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult UpdateCart(int RecordID, int quantity)
        {
            var CartItem = _unitOfWork.CartRepository.GetByID(RecordID);
            if (CartItem == null)
                return Json(new { result = 0 });
            CartItem.Count = quantity;
            _unitOfWork.SaveNotAync();
            return Json(new { result = 1 });
        }

        //
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult RemoveFromCart(int RecordID)
        {
            var cartItem = _unitOfWork.CartRepository.Get(
                cart => cart.CartID == GetCartId()
                && cart.RecordID == RecordID).SingleOrDefault();

            if (cartItem == null)
            {
                return Json(new { result = 0 });
            }
            _unitOfWork.CartRepository.Delete(cartItem);
            _unitOfWork.SaveNotAync();
            return Json(new { result = 1 });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult EmptyCart(string ListRecordID)
        {
            string[] list = ListRecordID.Split(',');
            var cartItems = _unitOfWork.CartRepository.Get(cart => cart.CartID == GetCartId());

            foreach (var cartItem in cartItems)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    try
                    {
                        if (cartItem.RecordID == int.Parse(list[i]))
                            _unitOfWork.CartRepository.Delete(cartItem);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            _unitOfWork.SaveNotAync();
            return Json(new { result = 1 });
        }
        public IEnumerable<Carts> GetCartItems()
        {
            return _unitOfWork.CartRepository.Get(cart => cart.CartID == _httpContextAccessor.HttpContext.Request.Cookies[CartCookieKey], includeProperties: "Products").ToList();
        }
        public int GetCount(string ShoppingCartId = "")
        {
            var count = (from cartItems in _unitOfWork.CartRepository.Get()
                         where cartItems.CartID == ShoppingCartId
                         select (int?)cartItems.Count).Sum();
            return Convert.ToInt32(count);
        }
        public decimal GetTotal()
        {
            var total = (from cartItems in _unitOfWork.CartRepository.Get()
                         where cartItems.CartID == _httpContextAccessor.HttpContext.Request.Cookies[CartCookieKey]
                         select (int?)cartItems.Count * cartItems.Price ?? cartItems.Products.Price).Sum();

            return Convert.ToDecimal(total);
        }
        public string GetCartId()
        {
            if (HttpContext.Request.Cookies != null && HttpContext.Request.Cookies[CartCookieKey] == null)
            {
                HttpContext.Response.Cookies.Append(CartCookieKey, Guid.NewGuid().ToString(),
                 new CookieOptions()
                 {
                     SameSite = SameSiteMode.Lax,
                     Secure = true,
                     Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                 });
            }
            return HttpContext.Request.Cookies[CartCookieKey];
        }
        public void MigrateCart(string userName, string ShoppingCartId = "")
        {
            var shoppingCart = _unitOfWork.CartRepository.Get(
                c => c.CartID == ShoppingCartId);

            foreach (var item in shoppingCart)
            {
                item.CartID = userName;
            }
            _unitOfWork.SaveNotAync();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}



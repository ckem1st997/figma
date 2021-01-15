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
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace figma.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailer _mailer;
        private const string CartCookieKey = "CartID";
        private static readonly ILog log =
          LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public VNPAY _vnpay { get; }

        public ShoppingCartController(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMailer mailer, IOptions<VNPAY> vnpay)
        {
            _vnpay = vnpay.Value;
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


        [Route("thanhtoan/nganhang")]
        public IActionResult ShopOnline(string orderID, decimal Price)
        {
            if (!(_unitOfWork.OrderRepository.Get(x => x.MaDonHang.Equals(orderID)).Count() == 1) || orderID == null)
                RedirectToAction("Index");
            ViewBag.id = orderID;
            ViewBag.price = Price;
            return View();
        }

        [Route("thanhtoan/nganhang")]
        [HttpPost]
        public IActionResult ShopOnline(OrderInfo orderInfo)
        {
            // Console.WriteLine(orderInfo.bank);
            //Get Config Info
            string vnp_Returnurl = _vnpay.vnp_Returnurl; //URL nhan ket qua tra ve 
            string vnp_Url = _vnpay.vnp_Url; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _vnpay.vnp_TmnCode; //Ma website
            string vnp_HashSecret = _vnpay.vnp_HashSecret; //Chuoi bi mat

            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = DateTime.Now.Ticks;
            order.Amount = Convert.ToDecimal(orderInfo.Amount);
            order.OrderDescription = orderInfo.OrderDescription;
            order.CreatedDate = DateTime.Now;

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            string locale = orderInfo.language;
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            Utils utils = new Utils(_httpContextAccessor);
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", orderInfo.OrderCatory); //default value: other
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress());
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));

            if (orderInfo.bank != null && !string.IsNullOrEmpty(orderInfo.bank))
            {
                vnpay.AddRequestData("vnp_BankCode", orderInfo.bank);
            }

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            Response.Redirect(paymentUrl);
            return View();
        }

        [BindProperties]
        public class GetRequest
        {
            public string vnp_Amount { get; set; }
            public string vnp_BankCode { get; set; }
            public string vnp_BankTranNo { get; set; }
            public string vnp_CardType { get; set; }
            public string vnp_OrderInfo { get; set; }
            public string vnp_PayDate { get; set; }
            public string vnp_ResponseCode { get; set; }
            public string vnp_TmnCode { get; set; }
            public string vnp_TransactionNo { get; set; }
            public string vnp_TxnRef { get; set; }
            public string vnp_SecureHashType { get; set; }
            public string vnp_SecureHash { get; set; }
        }


        public async Task<IActionResult> ResultATMPay(GetRequest getRequest)
        {
            log.InfoFormat("Begin VNPAY Return, URL={0}", Request.QueryString);
            if (Request.QueryString.Value.Length > 0 && getRequest.vnp_BankTranNo != null && getRequest.vnp_Amount.Length > 0 && getRequest.vnp_BankCode.Length > 0 && getRequest.vnp_CardType.Length > 0 && getRequest.vnp_OrderInfo.Length > 0 && getRequest.vnp_PayDate.Length > 0 && getRequest.vnp_ResponseCode.Length > 0 && getRequest.vnp_SecureHash.Length > 0 && getRequest.vnp_SecureHashType.Length > 0 && getRequest.vnp_TmnCode.Length > 0 && getRequest.vnp_TransactionNo.Length > 0 && getRequest.vnp_TxnRef.Length > 0)
            {
                string vnp_HashSecret = _vnpay.vnp_HashSecret; //Chuoi bi mat
                string vnpayData = Request.QueryString.ToString();
                VnPayLibrary vnpay = new VnPayLibrary();
                vnpay.AddResponseData("vnp_Amount", getRequest.vnp_Amount);
                vnpay.AddResponseData("vnp_BankCode", getRequest.vnp_BankCode);
                vnpay.AddResponseData("vnp_BankTranNo", getRequest.vnp_BankTranNo);
                vnpay.AddResponseData("vnp_CardType", getRequest.vnp_CardType);
                vnpay.AddResponseData("vnp_OrderInfo", getRequest.vnp_OrderInfo);
                vnpay.AddResponseData("vnp_PayDate", getRequest.vnp_PayDate);
                vnpay.AddResponseData("vnp_ResponseCode", getRequest.vnp_ResponseCode);
                vnpay.AddResponseData("vnp_SecureHash", getRequest.vnp_SecureHash);
                vnpay.AddResponseData("vnp_SecureHashType", getRequest.vnp_SecureHashType);
                vnpay.AddResponseData("vnp_TmnCode", getRequest.vnp_TmnCode);
                vnpay.AddResponseData("vnp_TransactionNo", getRequest.vnp_TransactionNo);
                vnpay.AddResponseData("vnp_TxnRef", getRequest.vnp_TxnRef);
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                //vnp_SecureHash: MD5 cua du lieu tra ve
                String vnp_SecureHash = getRequest.vnp_SecureHash;
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                EmptyCartRemove();
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        try
                        {
                            //Thanh toan thanh cong
                            ViewBag.thongbao = "Thanh toán thành công";
                            var ma = vnpay.GetResponseData("vnp_OrderInfo");
                            var order = _unitOfWork.OrderRepository.Get(x => x.MaDonHang.Equals(ma)).FirstOrDefault();
                            order.Payment = true;
                            order.ThanhToanTruoc = int.TryParse(vnpay.GetResponseData("vnp_Amount"), out int number) ? int.Parse(vnpay.GetResponseData("vnp_Amount")) / 100 : 0;
                            _unitOfWork.OrderRepository.Update(order);
                            _unitOfWork.SaveNotAync();
                            log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                            await _mailer.SendEmailSync(order.Email, "[" + order.MaDonHang + "] Đơn đặt hàng từ website ShopAsp.Net", "<p>Thanh toán đơn hàng thành công, số hoá đơn:" + orderId + ". Cảm ơn quý khách đã mua hàng !</p>");

                            var model = new CheckOutCompleteViewModel()
                            {
                                OrderID = ma,
                                Contact = _unitOfWork.ContactRepository.Get().FirstOrDefault()
                            };
                            return View(model);
                        }
                        catch (Exception)
                        {
                            ViewBag.thongbao = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                            log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                        }

                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.thongbao = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                }
                else
                {
                    log.InfoFormat("Invalid signature, InputData={0}", Request.QueryString);
                    ViewBag.thongbao = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            else
            {
                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                ViewBag.thongbao = "Lỗi, xin bạn vui lòng thử lại nha ";
                log.InfoFormat("Thanh toan loi");
            }

            return View();
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
                switch (model.Order.TypePay)
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
                switch (model.Order.Transport)
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

                sb.Append("<tr><td colspan='5' style='text-align:right'><strong>Tổng tiền (đã bao gồm cả phí ship): " + (tongtien < 500000 ? tongtien + 30000 : tongtien).ToString("N0") + " đ</strong></td></tr>");
                sb.Append("</table>");
                sb.Append("<p>Cảm ơn bạn đã tin tưởng và mua hàng của chúng tôi.</p>");
                await _mailer.SendEmailSync(model.Order.Email, "[" + model.Order.MaDonHang + "] Đơn đặt hàng từ website ShopAsp.Net", sb.ToString());
                if (model.Order.TypePay == 2)
                    return RedirectToAction("ShopOnline", new { orderID = model.Order.MaDonHang, Price = tongtien < 500000 ? tongtien + 30000 : tongtien });
                return RedirectToAction("CheckOutComplete", new { orderId = model.Order.MaDonHang });
            }
            model.Carts = GetCartItems();
            model.CartTotal = GetTotal() < 1000000 ? (GetTotal() + 30000) : GetTotal();
            return View(model);
        }

        [Route("thanh-toan-thanh-cong")]
        public ActionResult CheckOutComplete(string orderId)
        {
            EmptyCartRemove();
            var model = new CheckOutCompleteViewModel()
            {
                OrderID = orderId,
                Contact = _unitOfWork.ContactRepository.Get().FirstOrDefault()
            };
            return View(model);
        }


        public void EmptyCartRemove()
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



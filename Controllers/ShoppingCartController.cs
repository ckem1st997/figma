using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.DAL;
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
        private const string CartCookieKey = "CartID";

        public ShoppingCartController(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            if (!_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(CartCookieKey))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieKey, Guid.NewGuid().ToString(),
                 new CookieOptions()
                 {
                     SameSite = SameSiteMode.Lax,
                     Secure = true,
                     // hết hạn sau 1 day
                     Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                 });
            }
        }

        [Route("gio-hang/thong-tin")]
        public IActionResult Index(string returnUrl)
        {
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = GetCartItems(),
                CartTotal = GetTotal()
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult AddToCartAjax(int quantity, int productId, string color, string size)
        {
            decimal price = 0;
            var addedProduct = _unitOfWork.ProductRepository.Get(a => a.ProductID == productId).SingleOrDefault();
            Console.WriteLine(quantity);
            if (addedProduct == null)
            {
                return Json(new { result = 0, count = 0, productId, quantity, color, size });
            }

            if (addedProduct.SaleOff > 0)
            {
                price = addedProduct.SaleOff;
            }
            else if (addedProduct.Price > 0)
            {
                price = addedProduct.Price;
            }
            //Products product, decimal price, int quantity = 1, string color = null, string size = null)

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
        //catch (Exception)
        //{
        //    var data = new
        //    {
        //        result = quantity,
        //        count = productId,
        //        color = color,
        //        size = size
        //    };
        //    return Json(data);
        //}
        // }
        [HttpPost]
        public void AddProduct(int sid = 0, int pid = 0, int quantity = 0)
        {
            var product = _unitOfWork.ProductRepository.GetByID(pid);
            if (product == null) return;
            var cart = _unitOfWork.CartRepository.GetByID(sid);
            if (cart == null) return;
            cart.Count = quantity;
            _unitOfWork.SaveNotAync();
        }
        //[HttpPost]
        //public JsonResult RemoveFromCart(int id)
        //{
        //    // Remove the item from the cart
        //    var cart = ShoppingCart.GetCart(HttpContext);

        //    // Get the name of the album to display confirmation
        //    var productName = _unitOfWork.CartRepository.GetById(id).Product.Name;

        //    // Remove from cart
        //    var itemCount = cart.RemoveFromCart(id);

        //    // Display the confirmation message
        //    var results = new ShoppingCartRemoveViewModel
        //    {
        //        Message = productName + " đã được xóa khỏi giỏ hàng của bạn.",
        //        CartTotal = cart.GetTotal(),
        //        CartCount = cart.GetCount(),
        //        Status = itemCount,
        //        DeleteId = id
        //    };
        //    return Json(results);
        //}
        //public PartialViewResult CartSummary()
        //{
        //    var cart = ShoppingCart.GetCart(HttpContext);
        //    var model = new CartSummaryViewModel
        //    {
        //        Carts = cart.GetCartItems(),
        //        Count = cart.GetCount(),
        //        TotalMoney = cart.GetTotal()
        //    };
        //    return PartialView("CartSummary", model);
        //}




        //
        public int RemoveFromCart(int id, string ShoppingCartId = "")
        {
            var cartItem = _unitOfWork.CartRepository.Get(
                cart => cart.CartID == ShoppingCartId
                && cart.RecordID == id).SingleOrDefault();

            //var itemCount = 0;

            if (cartItem != null)
            {
                //if (cartItem.Count > 1)
                //{
                //    cartItem.Count--;
                //    itemCount = cartItem.Count;
                //}
                //else
                //{
                _unitOfWork.CartRepository.Delete(cartItem);
                //}
                _unitOfWork.SaveNotAync();
                return 1;
            }
            //return itemCount;
            return 0;
        }
        public void EmptyCart(string ShoppingCartId = "")
        {
            var cartItems = _unitOfWork.CartRepository.Get(cart => cart.CartID == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _unitOfWork.CartRepository.Delete(cartItem);
            }
            _unitOfWork.SaveNotAync();
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
        //public int CreateOrder(Order order)
        //{
        //    var cartItems = GetCartItems();
        //    foreach (var item in cartItems)
        //    {
        //        var orderDetail = new OrderDetail
        //        {
        //            ProductId = item.ProductID,
        //            OrderId = order.Id,
        //            Price = Convert.ToDecimal(item.Products.SaleOff ?? item.Products.Price),
        //            Quantity = item.Count
        //        };

        //        _unitOfWork.OrderDetailRepository.Insert(orderDetail);
        //    }
        //    _unitOfWork.Save();
        //    EmptyCart();
        //    return order.Id;
        //}
        public string GetCartId()
        {
            if (HttpContext.Request.Cookies != null && HttpContext.Request.Cookies[CartCookieKey] == null)
            {
                HttpContext.Response.Cookies.Append(CartCookieKey, Guid.NewGuid().ToString(),
                 new CookieOptions()
                 {
                     SameSite = SameSiteMode.Lax,
                     Secure = true,
                     // hết hạn sau 1 day
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


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}

//var claims = HttpContext.User.Claims;
//if (claims.Any())
//{
//    var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
//    if (userName != null)
//    {
//        HttpContext.Response.Cookies.Append(CartCookieKey, userName,
//          new CookieOptions()
//          {
//              SameSite = SameSiteMode.Lax,
//              Secure = true,
//              // hết hạn sau 1 day
//              Expires = new DateTimeOffset(DateTime.Now.AddDays(10))
//          });
//    }
//}
//else
//{


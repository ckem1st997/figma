using figma.DAL;
using figma.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ShoppingCart
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCart(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        //   string ShoppingCartId { get; set; }

        public const string CartCookieKey = "CartId";
        //public static ShoppingCart GetCart()
        //{
        //    shoppingCart.ShoppingCartId = GetCartId();
        //    return shoppingCart;
        //}
        //public static ShoppingCart GetCart(Controller controller)
        //{
        //    return GetCart(controller.HttpContext);
        //}
        public async void AddToCart(Products product, decimal price, int quantity = 1, string color = null, string size = null, string ShoppingCartId = "")
        {
            var cartItem = _unitOfWork.CartRepository.Get(c => c.CartID == ShoppingCartId && c.ProductID == product.ProductID).SingleOrDefault();

            if (cartItem == null)
            {
                cartItem = new Carts
                {
                    ProductID = product.ProductID,
                    Price = price,
                    CartID = ShoppingCartId,
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
            await _unitOfWork.Save();
        }
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
        public List<Carts> GetCartItems(string ShoppingCartId = "")
        {
            return _unitOfWork.CartRepository.Get(cart => cart.CartID == ShoppingCartId).ToList();
        }
        public int GetCount(string ShoppingCartId = "")
        {
            var count = (from cartItems in _unitOfWork.CartRepository.Get()
                         where cartItems.CartID == ShoppingCartId
                         select (int?)cartItems.Count).Sum();
            return Convert.ToInt32(count);
        }
        public decimal GetTotal(string ShoppingCartId = "")
        {
            var total = (from cartItems in _unitOfWork.CartRepository.Get()
                         where cartItems.CartID == ShoppingCartId
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
            if (_httpContextAccessor.HttpContext.Request.Cookies != null && _httpContextAccessor.HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains(CartCookieKey)).Value == null)
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
                if (userName != null)
                {
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieKey, userName,
                     new CookieOptions()
                     {
                         SameSite = SameSiteMode.Lax,
                         Secure = true,
                         // hết hạn sau 1 day
                         Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                     });
                }
                else
                {
                    var tempCartId = Guid.NewGuid();
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieKey, tempCartId.ToString(),
                    new CookieOptions()
                    {
                        SameSite = SameSiteMode.Lax,
                        Secure = true,
                        // hết hạn sau 1 day
                        Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                    });
                }
            }
            return _httpContextAccessor.HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains(CartCookieKey)).Value;
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
    }
}

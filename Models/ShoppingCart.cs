using figma.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public const string CartCookieKey = "CartId";

        public void AddToCart(Products product, decimal price, int quantity = 1, string color = null, string size = null, string ShoppingCartId = "")
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
            _unitOfWork.SaveNotAync();
        }
        public int RemoveFromCart(int id, string ShoppingCartId = "")
        {
            var cartItem = _unitOfWork.CartRepository.Get(
                cart => cart.CartID == ShoppingCartId
                && cart.RecordID == id).SingleOrDefault();



            if (cartItem != null)
            {
                _unitOfWork.CartRepository.Delete(cartItem);
                _unitOfWork.SaveNotAync();
                return 1;
            }
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

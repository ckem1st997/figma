using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using figma.Data;
using figma.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using figma.DAL;
using Microsoft.Extensions.Caching.Memory;

namespace figma.Controllers
{
    // [Developer("Joan Smith", "1", Reviewed = true)]
    //  [Developer("Joan Smith1", "2", Reviewed = false)]
    public class HomeController : Controller
    {
        //  private readonly ShopProductContext _context;
        private readonly UnitOfWork _unitOfWork;

        private IMemoryCache _iMemoryCache;
        public HomeController(UnitOfWork unitOfWork, IMemoryCache inMemoryCache)
        {
            _iMemoryCache = inMemoryCache;
            _unitOfWork = unitOfWork;
        }

        #region CustomAttribute
        //public static void GetAttribute(Type t)
        //{
        //    DeveloperAttribute[] MyAttributes =
        // (DeveloperAttribute[])Attribute.GetCustomAttributes(t, typeof(DeveloperAttribute));

        //    if (MyAttributes.Length == 0)
        //    {
        //        Console.WriteLine("The attribute was not found.");
        //    }
        //    else
        //    {
        //        for (int i = 0; i < MyAttributes.Length; i++)
        //        {
        //            // Get the Name value.
        //            Console.WriteLine("The Name Attribute is: {0}.", MyAttributes[i].Name);
        //            // Get the Level value.
        //            Console.WriteLine("The Level Attribute is: {0}.", MyAttributes[i].Level);
        //            // Get the Reviewed value.
        //            Console.WriteLine("The Reviewed Attribute is: {0}.", MyAttributes[i].Reviewed);
        //        }
        //    }
        //}
        #endregion

        //

        public IActionResult Index()
        {
            //  GetAttribute(typeof(HomeController));
            return View();
        }

        public IActionResult Login1()
        {
            return View();
        }
        [Authorize]
        public IActionResult Account()
        {

            var userName = HttpContext.Session.GetString("UserName");
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null && userName != null)
            {
                var result = _unitOfWork.MemberRepository.Get(a => a.MemberId == int.Parse(userId) && a.Email == userName);
                if (result != null)
                    return View(result);
            }
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        //
        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Bạn chưa nhập thông tin"), Display(Name = "Họ và tên"), MaxLength(50, ErrorMessage = "Họ và tên phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Họ và tên phải nhiều hơn 4 kí tự")]
            public string Fullname { get; set; }

            [Display(Name = "Điện thoại"), DataType(DataType.PhoneNumber, ErrorMessage = "Hãy nhập đúng số điện thoại")]
            public double Sdt { get; set; }

            [Required(ErrorMessage = "Bạn chưa nhập thông tin"), MaxLength(50), Display(Name = "Email"), DataType(DataType.EmailAddress)]
            public string Username { get; set; }

            [Required(ErrorMessage = "Bạn chưa nhập thông tin"), DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string Password { get; set; }

            [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Hai mật khẩu phải giống nhau"), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string ConfirmPassword { get; set; }
        }

        public class LoginViewModel
        {

            [Required, MaxLength(50), Display(Name = "Email"), DataType(DataType.EmailAddress)]
            public string Username { get; set; }

            [Required, DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string Password { get; set; }

            [Display(Name = "Nhớ mật khẩu")]
            public bool Remember { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([Bind] LoginViewModel user, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                if (ValidateAdmin(user.Username, user.Password))
                {
                    var users = _unitOfWork.MemberRepository.Get(a => a.Email == user.Username).SingleOrDefault();
                    if (users != null)
                    {
                        var userClaims = new List<Claim>()
                {
                    new Claim("UserName", users.Email),
                    new Claim("UserId", users.MemberId.ToString()),
                    new Claim(ClaimTypes.Actor, users.Active.ToString()),
                    new Claim(ClaimTypes.Role,users.Active?"Users":"Active"),
                    };
                        // add session
                        foreach (var item in userClaims)
                        {
                            HttpContext.Session.SetString(item.Type, item.Value);

                        }
                        var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity), authProperties);
                        //  code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                        //Execute("shoponline@gmail.com", "Shop1997", "hopxc1997@gmail.com", "Nguyễn Khả Hợp", "Thanh toán", "Đã mua").Wait();
                        //await _emailSender.SendEmailAsync("hopxc1997@gmail.com", "Confirm your email",
                        //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode("123")}'>clicking here</a>.");
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                           && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên đăng nhập không tồn tại");
                        return View(user);
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                    return View(user);
                }
            }
            return View(user);
        }

        [AllowAnonymous]
        public bool ValidateAdmin(string username, string password)
        {
            var admin = _unitOfWork.MemberRepository.Get(a => a.Email == username).SingleOrDefault();
            return admin != null && new PasswordHasher<Members>().VerifyHashedPassword(new Members(), admin.Password, password) == PasswordVerificationResult.Success;
        }
        //
        [AllowAnonymous]
        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dk = _unitOfWork.MemberRepository.Get(a => a.Email.Equals(model.Username)).SingleOrDefault();
                if (dk != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashedPassword = new PasswordHasher<Members>().HashPassword(new Members(), model.Password);
                    _unitOfWork.MemberRepository.Insert(new Members { Email = model.Username, Password = hashedPassword, Active = true, CreateDate = DateTime.Now, Fullname = model.Fullname, Mobile = model.Sdt.ToString(), Role = "User" });
                    await _unitOfWork.Save();
                    _iMemoryCache.Remove("Members");
                    TempData["tq"] = "Đăng ký thành công";
                    return RedirectToAction("Login", model);
                }
            }
            TempData["tq"] = "Thất bại";

            return View(model);
        }
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}

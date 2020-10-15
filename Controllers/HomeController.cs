using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using figma.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using figma.DAL;
using Microsoft.Extensions.Caching.Memory;
using figma.ViewModel;
using figma.OutFile;

namespace figma.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMemoryCache _iMemoryCache;
        public HomeController(UnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _iMemoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Products = _unitOfWork.ProductRepository.Get(a => a.Active, q => q.OrderBy(a => a.Sort), 12),
                Banners = _unitOfWork.BannerRepository.Get(a => a.Active, q => q.OrderBy(a => a.Soft)),
                ConfigSites = _unitOfWork.ConfigSiteRepository.Get()
            };
            return View(model);
        }

        [Route("{name}-{proId}.html")]
        public IActionResult Product(int proId = 0)
        {
            HttpContext.Response.Cookies.Append(
                     "viewProducts", "" + HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value + "," + proId + "",
                     new CookieOptions()
                     {
                         SameSite = SameSiteMode.Lax,
                         Secure = true,
                         Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                     });
            ViewBag.view = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value;
            var product = _unitOfWork.ProductRepository.GetByID(proId);
            if (product == null)
            {
                return RedirectToActionPermanent("Index");
            }
            var products = _unitOfWork.ProductRepository.Get(
                a => a.Active && a.ProductCategorieID == product.ProductCategorieID && a.ProductID != proId,
                q => q.OrderByDescending(a => a.Sort), 8);
            var model = new ProductDetailViewModel
            {
                Product = product,
                Products = products,
                ViewProducts = _unitOfWork.ProductRepository.Get().ToList(),
                RootCategory = _unitOfWork.ProductCategoryRepository.Get(a => a.Active, q => q.OrderByDescending(a => a.Soft)).SingleOrDefault(a => a.ProductCategorieID == product.ProductCategorieID),
                Collection = _unitOfWork.CollectionRepository.Get(a => a.Active).SingleOrDefault(a => a.CollectionID == product.CollectionID),
                TagProducts = _unitOfWork.TagsProductsRepository.Get(a => a.ProductID == proId, includeProperties: "Tags,Products"),
                ProductSizeColors = _unitOfWork.ProductSCRepository.Get(includeProperties: "Size,Color").ToList()
            };
            return View(model);
        }


        [Route("{namearicle}/{name}-{blogId}.html")]
        public IActionResult Review(string namearicle, int blogId = 0)
        {
            var ar = _unitOfWork.ArticleRepository.GetByID(blogId);
            var listAr = _unitOfWork.ArticleRepository.Get(a => a.Active && a.Home && a.Id != ar.Id && a.ArticleCategoryId == ar.ArticleCategoryId, q => q.OrderBy(a => a.CreateDate), records: 4);
            ViewBag.namearicle = namearicle;
            var model = new ReviewViewModel()
            {
                Article = ar,
                Articles = listAr
            };
            return View(model);
        }

        [Route("ListReview")]
        public IActionResult ListReview()
        {
            var model = new ReviewViewModel()
            {
                Articles = _unitOfWork.ArticleRepository.Get(),
                ListArticles = _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive && a.ShowHome && a.ShowHome, records: 30)
            };

            return View(model);
        }

        //
        [Route("collections/{name}-{catId}")]
        public async Task<IActionResult> Info(int catId, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var category = _unitOfWork.ProductCategoryRepository.GetByID(catId);
            if (category == null)
            {
                return RedirectToActionPermanent("Index");
            }
            ViewBag.category = category;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var Products = await _unitOfWork.ProductRepository.GetAync(a => a.Active && (a.ProductCategorieID == catId || a.ProductCategories.ParentId == catId), orderBy: q => q.OrderBy(a => a.Name));
            if (!String.IsNullOrEmpty(searchString))
            {
                Products = Products.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Products = Products.OrderByDescending(a => a.Name);
                    break;
                case "Date":
                    Products = Products.OrderBy(a => a.CreateDate);
                    break;
                case "date_desc":
                    Products = Products.OrderByDescending(a => a.CreateDate);
                    break;
                case "Price":
                    Products = Products.OrderBy(a => a.Price);
                    break;
                case "price_desc":
                    Products = Products.OrderByDescending(a => a.Price);
                    break;
                default:
                    Products = Products.OrderBy(a => a.Name);
                    break;
            }
            int pageSize = 8;
            return View(PaginatedList<Products>.CreateAsync(Products, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Search(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var Products = await _unitOfWork.ProductRepository.GetAync(a => a.Active && a.Name.Contains(searchString), orderBy: q => q.OrderBy(a => a.Name));
            switch (sortOrder)
            {
                case "name_desc":
                    Products = Products.OrderByDescending(a => a.Name);
                    break;
                case "Date":
                    Products = Products.OrderBy(a => a.CreateDate);
                    break;
                case "date_desc":
                    Products = Products.OrderByDescending(a => a.CreateDate);
                    break;
                case "Price":
                    Products = Products.OrderBy(a => a.Price);
                    break;
                case "price_desc":
                    Products = Products.OrderByDescending(a => a.Price);
                    break;
                default:
                    Products = Products.OrderBy(a => a.Name);
                    break;
            }
            int pageSize = 8;
            return View(PaginatedList<Products>.CreateAsync(Products, pageNumber ?? 1, pageSize));
        }


        #region Account
        [Authorize]
        public IActionResult Account()
        {
            var claims = HttpContext.User.Claims;
            var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
            var userId = claims.FirstOrDefault(c => c.Type == "UserId").Value;
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
                        var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity), authProperties);
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


        [HttpPost]
        [AllowAnonymous]
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
                    return RedirectToAction("Login");
                }
            }
            TempData["tq"] = "Thất bại";

            return View(model);
        }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Bạn chưa nhập thông tin"), Display(Name = "Họ và tên"), MaxLength(50, ErrorMessage = "Họ và tên phải ít hơn 50 kí tự"), MinLength(5, ErrorMessage = "Họ và tên phải nhiều hơn 4 kí tự"), RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s]+$",
         ErrorMessage = "Tên đăng nhập chỉ chấp nhận chữ cái !")]
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
        #endregion
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}

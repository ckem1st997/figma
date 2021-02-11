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
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using figma.Interface;
using Hangfire;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Google;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace figma.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMailer _mailer;
        private readonly IHttpClientFactory _clientFactory;
        private const string CartCookieKey = "CartID";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(UnitOfWork unitOfWork, IMailer mailer, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccesso)
        {
            _httpContextAccessor = httpContextAccesso;
            _unitOfWork = unitOfWork;
            _mailer = mailer;
            _clientFactory = clientFactory;
            if (_httpContextAccessor.HttpContext.Request.Cookies[CartCookieKey] == null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieKey, Guid.NewGuid().ToString(),
                 new CookieOptions()
                 {
                     //  SameSite = SameSiteMode.Lax,
                     // Secure = true,
                     Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                 });
            }
        }
        public JsonResult Add()
        {
            //ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,
            //Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,
            //DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy
            //string name = "Váy STELLA BE cổ vuông chun vai";
            //string[] img = { "uploads/2020/09/06/132438363534870618.png,uploads/2020/09/06/132438363539334740.png,uploads/2020/09/06/132438363541560735.png,uploads/2020/09/06/132438363544079376.png,uploads/2020/09/06/132438363547063270.png,uploads/2020/09/06/132438363549254747.png", "uploads/2020/09/03/132435734556263309.png,uploads/2020/09/03/132435734559207086.png,uploads/2020/09/03/132435734561526431.png,uploads/2020/09/03/132435734565776953.png,uploads/2020/09/03/132435734568138937.png,uploads/2020/09/03/132435734570197372.png,uploads/2020/09/03/132435734572503314.png", "uploads/2020/09/03/132435734932870348.png,uploads/2020/09/03/132435734935739906.png,uploads/2020/09/03/132435734938064048.png,uploads/2020/09/03/132435734939979281.png,uploads/2020/09/03/132435734942204032.png,uploads/2020/09/03/132435734944396254.png,uploads/2020/09/03/132435734946580826.png,uploads/2020/09/03/132435734948894694.png", "uploads/2020/09/03/132435735253593281.png,uploads/2020/09/03/132435735256435652.png,uploads/2020/09/03/132435735258660281.png,uploads/2020/09/03/132435735260562887.png,uploads/2020/09/03/132435735262847997.png,uploads/2020/09/03/132435735265158821.png,uploads/2020/09/03/132435735267492917.png", "uploads/2020/09/03/132435735563298472.png,uploads/2020/09/03/132435735566364804.png,uploads/2020/09/03/132435735568691340.png,uploads/2020/09/03/132435735570584196.png,uploads/2020/09/03/132435735572904220.png,uploads/2020/09/03/132435735575345447.png,uploads/2020/09/03/132435735577225153.png", "uploads/2020/09/03/132435735951790896.png,uploads/2020/09/03/132435735954234595.png,uploads/2020/09/03/132435735956085355.png,uploads/2020/09/03/132435735958246157.png,uploads/2020/09/03/132435735960310306.png,uploads/2020/09/03/132435735962146364.png", "uploads/2020/09/03/132435736366333089.png,uploads/2020/09/03/132435736369311489.png,uploads/2020/09/03/132435736371620431.png,uploads/2020/09/03/132435736373535857.png,uploads/2020/09/03/132435736375898195.png,uploads/2020/09/03/132435736378024042.png,uploads/2020/09/03/132435736380043009.png", "uploads/2020/09/03/132435736705873514.png,uploads/2020/09/03/132435736708922105.png,uploads/2020/09/03/132435736711443768.png,uploads/2020/09/03/132435736713435462.png,uploads/2020/09/03/132435736715715778.png,uploads/2020/09/03/132435736717949521.png,uploads/2020/09/03/132435736719937440.png", "uploads/2020/09/20/132450508499578916.jpg,uploads/2020/09/20/132450508503182823.jpg,uploads/2020/09/20/132450508504649804.jpg,uploads/2020/09/20/132450508506052883.jpg,uploads/2020/09/20/132450508507255832.jpg,uploads/2020/09/20/132450508507905622.jpg,uploads/2020/09/20/132450508508454478.jpg", "uploads/2020/09/20/132450509184786404.jpg,uploads/2020/09/20/132450509185535127.jpg,uploads/2020/09/20/132450509186245184.jpg,uploads/2020/09/20/132450509186811881.jpg,uploads/2020/09/20/132450509187311790.jpg,uploads/2020/09/20/132450509187834360.jpg,uploads/2020/09/20/132450509188318371.jpg", "uploads/2020/09/20/132450509925989910.jpg,uploads/2020/09/20/132450509926969849.jpg,uploads/2020/09/20/132450509928094111.jpg,uploads/2020/09/20/132450509928744734.jpg,uploads/2020/09/20/132450509929254189.jpg,uploads/2020/09/20/132450509929777010.jpg,uploads/2020/09/20/132450509930267158.jpg", "uploads/2020/09/20/132450510235331261.jpg,uploads/2020/09/20/132450510236311537.jpg,uploads/2020/09/20/132450510237067016.jpg,uploads/2020/09/20/132450510237643353.jpg,uploads/2020/09/20/132450510238144788.jpg,uploads/2020/09/20/132450510238660206.jpg,uploads/2020/09/20/132450510239167509.jpg", "uploads/2020/09/20/132450510569896054.jpg,uploads/2020/09/20/132450510570666055.jpg,uploads/2020/09/20/132450510571453837.jpg,uploads/2020/09/20/132450510572185988.jpg,uploads/2020/09/20/132450510572686764.jpg,uploads/2020/09/20/132450510573330928.jpg,uploads/2020/09/20/132450510573854815.jpg", "uploads/2020/09/20/132450510941022590.jpg,uploads/2020/09/20/132450510942112914.jpg,uploads/2020/09/20/132450510942855545.jpg,uploads/2020/09/20/132450510943481997.jpg,uploads/2020/09/20/132450510944116804.jpg,uploads/2020/09/20/132450510944732560.jpg,uploads/2020/09/20/132450510945248503.jpg", "uploads/2020/09/20/132450513718160233.jpg,uploads/2020/09/20/132450513718933077.jpg,uploads/2020/09/20/132450513719401577.jpg,uploads/2020/09/20/132450513719919186.jpg", "uploads/2020/09/20/132450513942758055.jpg,uploads/2020/09/20/132450513943699541.jpg,uploads/2020/09/20/132450513944219267.jpg,uploads/2020/09/20/132450513944597805.jpg,uploads/2020/09/20/132450513944973670.jpg,uploads/2020/09/20/132450513945533886.jpg" };
            //string Description = "Váy STELLA BE cổ vuông chun vai";
            //int ProductCategorieID = 6;
            //int Quantity = 1;
            //string[] Factory = { "Việt Nam", "Mỹ", "Hàn Quốc" };
            //decimal Price = 600000;
            //decimal SaleOff = 100000;
            //string[] CreateBy = { "Admin", "NV1", "NV2" };
            //int Home = 1;
            //int Sort = 1;
            //int active = 1;
            //string TitleMeta = "váy cao cấp 2018, rèm vải đẹp hà nội";
            //int CollectionID = 2;
            //double BarCode = 1;
            //bool[] Hot = { true, false };
            //int StatusProduct = 1;
            //string[] Body = { "", "" };
            //string DescriptionMeta = "Rèm vải đẹp, mẫu mã đa dạng, Rèm Nam An –đơn vị cung cấp rèm uy tin chất lượng, bảo hành trọn đời sản phẩm. Giảm giá ngay 10% khi đặt hàng online. Miễn phí phụ kiện và công lắp đặt.";
            try
            {
                for (int i = 1; i < 12224; i++)
                {
                    // Random random = new Random();
                    //  Products products = new Products();
                    //products.Name = name + "" + i + "";
                    //products.Image = img[random.Next(0, img.Length)];
                    //products.Description = Description + "" + i + "";
                    //products.ProductCategorieID = 6;
                    //products.Quantity = random.Next(1, 1000);
                    //products.Factory = Factory[random.Next(0, Factory.Length)];
                    //products.Price = random.Next(100000, 1000000);
                    //products.SaleOff = random.Next(0, 100000);
                    //products.CreateBy = CreateBy[random.Next(0, CreateBy.Length)];
                    //products.Home = true;
                    //products.Sort = _unitOfWork.ProductRepository.Get().Count() + i;
                    //products.Active = true;
                    //products.TitleMeta = TitleMeta;
                    //products.CollectionID = 2;
                    //products.BarCode = random.Next(100000000, 999999999).ToString();
                    //products.Hot = Hot[random.Next(0, Hot.Length)];
                    //products.StatusProduct = true;
                    //   products.Description = DescriptionMeta;
                    var products = _unitOfWork.ProductRepository.GetByID(i);
                    if (products != null)
                    {
                        products.Description = "SP-ASP.NET" + i + "";
                        _unitOfWork.ProductRepository.Update(products);
                        _unitOfWork.SaveNotAync();
                        Console.WriteLine(i);
                    }
                    else
                        continue;
                }

                return Json(new { result = 1 });
            }
            catch (Exception)
            {

                return Json(new { result = 2 });
            }
        }

        public IActionResult Chat()
        {
            return View();
        }


        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Products = _unitOfWork.ProductRepository.Get(a => a.Active, q => q.OrderBy(a => a.Sort), 12).Select(y => new ViewProducts
                {
                    Name = y.Name,
                    CreateDate = y.CreateDate,
                    Hot = y.Hot,
                    Image = y.Image,
                    Price = y.Price,
                    ProductID = y.ProductID,
                    SaleOff = y.SaleOff,
                    Sort = y.Sort,
                    Quantity = y.Quantity
                }),
                Banners = _unitOfWork.BannerRepository.Get(a => a.Active, q => q.OrderBy(a => a.Soft)),
                ConfigSites = _unitOfWork.ConfigSiteRepository.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> GoogleLogin(string Token)
        {
            Console.WriteLine(Token);
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(Token);
            await AddCookieAuthor(payload.Name, payload.Subject, "Google");

            return Ok(true);
        }

        private async Task AddCookieAuthor(string name, string id, string social)
        {
            var userClaims = new List<Claim>()
                {
                    new Claim("UserName", name),
                    new Claim("UserId",id),
                    new Claim(ClaimTypes.Actor, "true"),
                    new Claim(ClaimTypes.Role,"Users"),
                    new Claim("Social",social),
                    };
            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity), authProperties);
        }

        public class JsonFB
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }
        //fb
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> FaceBookLogin(string accessToken, string userID, string graphDomain)
        {

            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://graph.facebook.com/" + userID + "?fields=name,email&access_token=" + accessToken + "");
            var requestName = new HttpRequestMessage(HttpMethod.Get,
                "https://graph.facebook.com/v9.0/me?access_token=" + accessToken + "&method=get&pretty=0&sdk=joey&suppress_http_code=1");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var responsename = await client.SendAsync(requestName);
            string idss = "";
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                foreach (var item in json)
                {
                    if (item.Key.Equals("id"))
                    {
                        idss = item.Value.ToString();
                        break;
                    }
                }
                var resultname = await responsename.Content.ReadAsStringAsync();
                JObject json1 = JObject.Parse(resultname);
                JsonFB j1 = new JsonFB();
                JsonFB j2 = new JsonFB();
                GetValueJson(json, j1);
                GetValueJson(json1, j2);
                if (idss.Equals(userID) && graphDomain.Equals("facebook") && j1.Id.Equals(j2.Id) && j1.Name.Equals(j2.Name))
                {
                    await AddCookieAuthor(j1.Name, j1.Id, "Facebook");
                    return Ok(true);
                }
            }
            return Ok(false);
        }

        private static JsonFB GetValueJson(JObject json, JsonFB j1)
        {
            foreach (var item in json)
            {
                if (item.Key.Equals("id"))
                    j1.Id = item.Value.ToString();
                if (item.Key.Equals("name"))
                    j1.Name = item.Value.ToString();
            }

            return j1;
        }

        #region laylaipw
        public class PasswordEmailSend
        {
            [Required, DataType(DataType.EmailAddress)]
            public string email { get; set; }
        }
        [HttpGet]
        public IActionResult PasswordEmail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PasswordEmailXN()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PasswordEmailTwo(string email)
        {
            ViewBag.email = email;
            return View();
        }

        //   [Route("PasswordEmail/email={email}")]
        [HttpPost]
        public async Task<IActionResult> PasswordEmail(PasswordEmailSend send)
        {
            if (!ModelState.IsValid)
                if (send.email == null)
                    return RedirectToAction("PasswordEmail");
            var user = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(send.email)).FirstOrDefault();
            if (user == null)
                return RedirectToAction("PasswordEmail");

            string body = "<a href='http://" + Request.Host.Value + "/Home/PasswordEmailLink/" + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(send.email)) + "' target = '_blank' ><span style = 'color:blue'>Click xác nhận Email</span></a>";
            await _mailer.SendEmailSync(send.email, "Email xác nhận lấy lại mật khẩu từ website ShopAsp.Net", body);
            return RedirectToAction("PasswordEmailTwo", new { email = send.email });

        }

        // [Route("Home/PasswordEmailLink/email={email}")]
        public IActionResult PasswordEmailLink(string id)
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(id));
            var user = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(code)).FirstOrDefault();
            HttpContext.Session.Remove("repw");
            HttpContext.Session.Set("repw", Encoding.ASCII.GetBytes(id));
            if (user == null || code == null)
                return NotFound($"Hông tìm thấy Email '{id}'");
            ViewBag.email = code;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordEmailLink(string id, [Bind("Username", "Password", "ConfirmPassword")] ConfrimViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(model.Username)).FirstOrDefault();
                    var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Encoding.ASCII.GetString(HttpContext.Session.Get("repw"))));
                    if (user != null && email.Equals(model.Username))
                    {
                        var hashedPassword = new PasswordHasher<Members>().HashPassword(new Members(), model.Password);
                        Members members = new Members();
                        members = user;
                        members.Password = hashedPassword;
                        _unitOfWork.MemberRepository.Update(members);
                        await _unitOfWork.Save();
                        return RedirectToAction(nameof(PasswordEmailXN));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(PasswordEmail));
            }
            ViewBag.email = model.Username;
            return View(model);
        }
        #endregion
        #region EmailXacNhan
        public string UrlConfirmEmail(string email)
        {
            if (email == null)
                return "";
            Guid g = Guid.NewGuid();
            var EmailConfirmationUrl = "/EmailConfirmation/token=" + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(UpdateToken(email, g.ToString()))) + "&code=" + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(email)) + "";
            var jobId = BackgroundJob.Schedule(() => UpdateToken(email, null), TimeSpan.FromSeconds(60));
            return EmailConfirmationUrl;
        }

        public string UpdateToken(string email, string g)
        {
            Members members = new Members();
            members = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(email), records: 1).FirstOrDefault();
            members.token = g;
            //   members.ConfirmEmail = false;
            _unitOfWork.MemberRepository.Update(members);
            _unitOfWork.SaveNotAync();
            return g;
        }

        //xác nhận
        [Route("EmailConfirmation/token={token}&code={code}")]
        public async Task<IActionResult> EmailConfirmation(string token, string code)
        {
            var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var tokencode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            if (email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(email) && x.token.Equals(tokencode)).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("SendEmailConfirmation", new { email = email });
            }
            user.ConfirmEmail = true;
            user.token = null;
            _unitOfWork.MemberRepository.Update(user);
            await _unitOfWork.Save();
            return View(user);
        }


        // send token
        [Route("SendEmailConfirmation/email={email}")]
        public IActionResult SendEmailConfirmation(string email)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(email)).FirstOrDefault();
            if (user == null)
            {
                return NotFound($"Tài khoản '{email}': chưa được đăng ký !");
            }
            ViewBag.email = email;
            return View();
        }

        [Route("SendEmail/email={email}")]
        public async Task<IActionResult> SendEmailAsync(string email)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = _unitOfWork.MemberRepository.Get(x => x.Email.Equals(email)).FirstOrDefault();
            if (user == null)
            {
                return NotFound($"Tài khoản '{email}': chưa được đăng ký !");
            }
            await SendEmailConfrim(email);
            return RedirectToAction("ConfirmEmail", new { email = email });
        }
        public IActionResult ConfirmEmail(string email)
        {
            if (email == null)
                return NotFound($"Unable to load user with email '{email}'.");
            ViewBag.email = email;
            return View();
        }
        private async Task SendEmailConfrim(string email)
        {
            string body = "<a href='http://" + Request.Host.Value + "" + UrlConfirmEmail(email) + "' target = '_blank' ><span style = 'color:blue'>Click xác nhận Email</span></a>";
            await _mailer.SendEmailSync(email, "Email xác nhận đăng ký tài khoản từ website ShopAsp.Net", body);
        }

        #endregion
        [Route("{name}-{proId}.html")]
        public IActionResult Product(int proId = 0)
        {
            try
            {
                var list = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value;
                if (list.IndexOf(proId.ToString()) == -1)
                    HttpContext.Response.Cookies.Append(
                             "viewProducts", "" + HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value + "," + proId + "",
                             new CookieOptions()
                             {
                                 //SameSite = SameSiteMode.Lax,
                                 //Secure = true,
                                 Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                             });
                //  ViewBag.view = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value;
            }
            catch (Exception)
            {
                HttpContext.Response.Cookies.Append(
                             "viewProducts", "" + proId + "",
                             new CookieOptions()
                             {
                                 Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
                             });
            }

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
                //   ViewProducts = _unitOfWork.ProductRepository.Get().ToList(),
                RootCategory = _unitOfWork.ProductCategoryRepository.Get(a => a.Active, q => q.OrderByDescending(a => a.Soft)).SingleOrDefault(a => a.ProductCategorieID == product.ProductCategorieID),
                Collection = _unitOfWork.CollectionRepository.Get(a => a.Active).SingleOrDefault(a => a.CollectionID == product.CollectionID),
                TagProducts = _unitOfWork.TagsProductsRepository.Get(a => a.ProductID == proId, includeProperties: "Tags,Products"),
                //  ProductSizeColors = _unitOfWork.ProductSCRepository.Get(includeProperties: "Size,Color").ToList(),
                GetColors = _unitOfWork.ProductSCRepository.Get(x => x.ProductID == proId && !x.Color.NameColor.Equals("Không màu"), includeProperties: "Color").GroupBy(x => new { x.Color.NameColor, x.Color.Code }).Select(y => new GetColorId { NameColor = y.Key.NameColor, Code = y.Key.Code }),
                GetSizes = _unitOfWork.ProductSCRepository.Get(x => x.ProductID == proId && !x.Size.SizeProduct.Equals("Không Size"), includeProperties: "Size").GroupBy(x => x.Size.SizeProduct).Select(y => new GetSizeId { SizeProduc = y.Key.Trim() })
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

        [Route("tra-cuu-don-hang")]
        public IActionResult OrderDetails(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var result = _unitOfWork.OrderRepository.Get(a => a.MaDonHang.Equals(search) || a.Email.Equals(search) || a.Mobile.Equals(search), includeProperties: "OrderDetails");
                return View(result);
            }
            return View();
        }


        public class OrderDetailsSearch
        {
            public Order Orders { get; set; }
            public decimal Price { get; set; }
        };
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
            Products = sortOrder switch
            {
                "name_desc" => Products.OrderByDescending(a => a.Name),
                "Date" => Products.OrderBy(a => a.CreateDate),
                "date_desc" => Products.OrderByDescending(a => a.CreateDate),
                "Price" => Products.OrderBy(a => a.Price),
                "price_desc" => Products.OrderByDescending(a => a.Price),
                _ => Products.OrderBy(a => a.Name),
            };
            IEnumerable<ViewProducts> view = Products.Select(y => new ViewProducts
            {
                Name = y.Name,
                CreateDate = y.CreateDate,
                Hot = y.Hot,
                Image = y.Image,
                Price = y.Price,
                ProductID = y.ProductID,
                SaleOff = y.SaleOff,
                Sort = y.Sort,
                Quantity = y.Quantity
            });
            int pageSize = 20;
            return View(PaginatedList<ViewProducts>.CreateAsync(view, pageNumber ?? 1, pageSize));
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
            Products = sortOrder switch
            {
                "name_desc" => Products.OrderByDescending(a => a.Name),
                "Date" => Products.OrderBy(a => a.CreateDate),
                "date_desc" => Products.OrderByDescending(a => a.CreateDate),
                "Price" => Products.OrderBy(a => a.Price),
                "price_desc" => Products.OrderByDescending(a => a.Price),
                _ => Products.OrderBy(a => a.Name),
            };
            int pageSize = 8;
            return View(PaginatedList<Products>.CreateAsync(Products, pageNumber ?? 1, pageSize));
        }


        public class SocialInfomation
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Social { get; set; }
        }

        #region Account
        [Authorize]
        public async Task<IActionResult> Account()
        {
            AccountViewModel model = new AccountViewModel();
            var claims = HttpContext.User.Claims;
            var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
            var userId = claims.FirstOrDefault(c => c.Type == "UserId").Value;
            if (userId != null && userName != null)
            {
                try
                {
                    ViewBag.name = userName;
                    ViewBag.id = userId;
                    ViewBag.social = claims.FirstOrDefault(c => c.Type == "Social").Value;
                    var result = _unitOfWork.MemberRepository.GetByID(int.Parse(userId));
                    model.Members = result;
                    if (result != null)
                        return View(model);
                }
                catch (Exception)
                {

                    return View();
                }

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
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                if (ValidateAdmin(user.Username, user.Password))
                {
                    var users = _unitOfWork.MemberRepository.Get(a => a.Email == user.Username).SingleOrDefault();
                    if (users != null)
                    {
                        if (users.ConfirmEmail && users.Active)
                        {
                            var userClaims = new List<Claim>()
                        {
                                new Claim("UserName", users.Email),
                                new Claim("UserId", users.MemberId.ToString()),
                                new Claim(ClaimTypes.Actor, users.Active.ToString()),
                                new Claim(ClaimTypes.Role,users.Active?"Users":"Active"),
                               new Claim("Social","none")
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
                        else if (!users.ConfirmEmail)
                        {
                            return RedirectToAction(nameof(SendEmailConfirmation), new { email = users.Email });
                        }
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
            var admin = _unitOfWork.MemberRepository.Get(a => a.Email.Equals(username)).SingleOrDefault();
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
                    TempData["tq"] = "Đăng ký thành công";
                    await SendEmailConfrim(model.Username);
                    return RedirectToAction("ConfirmEmail", new { email = model.Username });
                }
            }
            TempData["tq"] = "Thất bại";

            return View(model);
        }
        // like Products
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult LikeProducts(int productid)
        {
            return Ok(productid);
        }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Họ và tên hông được để trống nha !"), Display(Name = "Họ và tên"), MaxLength(50, ErrorMessage = "Họ và tên phải ít hơn 50 kí tự"), MinLength(5, ErrorMessage = "Họ và tên phải nhiều hơn 4 kí tự"), RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s]+$",
         ErrorMessage = "Họ và tên chỉ chấp nhận chữ cái !")]
            public string Fullname { get; set; }
            //^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$
            //(0)+([0-9]{9})\b
            [Display(Name = "Điện thoại"), Required(ErrorMessage = "Điện thoại hông được để trống nha !"), RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[0-9]{6}$", ErrorMessage = "Hãy nhập đúng số điện thoại chỉ bao gồm 10 kí tự số")]
            public double Sdt { get; set; }

            [Required(ErrorMessage = "Tài khoản hông được để trống nha !"), MaxLength(50), Display(Name = "Email"), DataType(DataType.EmailAddress)]
            public string Username { get; set; }

            [Required(ErrorMessage = "Mật khẩu hông được để trống nha !"), DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Mật khẩu nhập lại hông được để trống nha !"), DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Hai mật khẩu phải giống nhau"), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string ConfirmPassword { get; set; }
        }

        public class ConfrimViewModel
        {
            [Required(ErrorMessage = "Tài khoản hông được để trống nha !"), MaxLength(50), Display(Name = "Email"), DataType(DataType.EmailAddress)]
            public string Username { get; set; }

            [Required(ErrorMessage = "Mật khẩu hông được để trống nha !"), DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Mật khẩu nhập lại hông được để trống nha !"), DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Hai mật khẩu phải giống nhau"), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string ConfirmPassword { get; set; }
        }

        public class getIDMember
        {
            public int MemberId { get; set; }
        }
        public class LoginViewModel
        {

            [Required(ErrorMessage = "Xin vui lòng nhập Email !"), MaxLength(50), Display(Name = "Email"), DataType(DataType.EmailAddress)]
            public string Username { get; set; }

            [Required(ErrorMessage = "Xin vui lòng nhập mật khẩu !"), DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
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
//{
//  "VNPAY:vnp_Url": "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
//  "VNPAY:vnp_TmnCode": "JAQ4YLZE",
//  "VNPAY:vnp_Returnurl": "https://localhost:44302/ShoppingCart/ResultATMPay",
//  "VNPAY:vnp_HashSecret": "AMNIWKSEYHVVGGVAVTAASDVCLVQCBUNU",
//  "VNPAY:vnpay_api_url": "https://merchant.vnpay.vn/merchant_webapi/merchant.html",
//  "Smtp:Username": "nguyenkhahop1997@gmail.com",
//  "Smtp:Server": "smtp.gmail.com",
//  "Smtp:SenderName": "Nhân viên",
//  "Smtp:SenderEmail": "shoponline@gmail.com",
//  "Smtp:Port": "25",
//  "Smtp:Password": "0977751021",
//  "Firebase:Bucket": "uploadimage-292509.appspot.com",
//  "Firebase:AuthPassword": "0977751021",
//  "Firebase:AuthEmail": "nguyenkhahop1997@gmail.com",
//  "Firebase:ApiKey": "AIzaSyCeW-W94Wl_tFOaWzjIGOCj-tPPkAhcKtQ",
//  "ConnectionStrings:ShopProductContext": "Data Source=DESKTOP-ITLR9T6;Initial Catalog=sh3;Integrated Security=True",
//  "ConnectionStrings:ShopProductContext": "Data Source=localhost;Initial Catalog=sh3;Integrated Security=True",
//  "ConnectionStrings:ShopProductContext": "Data Source=SQL5102.site4now.net,1433;Initial Catalog=DB_A6EF04_meoh0ang97;User Id=DB_A6EF04_meoh0ang97_admin;Password=Aa0977751021",
//  "Authentication:Google:ClientSecret": "0dPst1kGqeZauftTQ-va7HrS",
//  "Authentication:Google:ClientId": "936508163061-721m10k2fer4grtfpp1bkun7greim7fu.apps.googleusercontent.com",
//  "Authentication:Facebook:AppSecret": "afc462708091d6188c903e76f6b45771",
//  "Authentication:Facebook:AppId": "826080674623621",
//  "Kestrel:Certificates:Development:Password": "bbed3394-bd97-4649-81d0-9c672cebbb6e"
//}
//Password_01

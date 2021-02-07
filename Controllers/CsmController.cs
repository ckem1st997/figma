using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using figma.Models;
using figma.OutFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using figma.ViewModel;
using figma.DAL;
using System.Globalization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace figma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CsmController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CsmController(IWebHostEnvironment hostEnvironment, UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        public IActionResult DeleteImage(string filesadd)
        {
            var result = false;
            if (filesadd != null)
            {
                String filepath = Path.Combine(_hostingEnvironment.WebRootPath, filesadd);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    result = true;
                }
            }
            else
                return Ok(new { result, content = "Xóa thất bại !" });
            return Ok(new { result, content = "Xóa thành công !" });

        }
        //
        #region Products

        public IActionResult ListProducts()
        {
            var shopProductContext = _unitOfWork.ProductRepository.Get(includeProperties: "Collection,ProductCategories",records:10);
            return View(shopProductContext.ToList());
        }

        public IActionResult ProductsCreate()
        {
            ViewData["CollectionID"] = new SelectList(_unitOfWork.CollectionRepository.Get().ToList(), "CollectionID", "Name");
            ViewData["ProductCategorieID"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get().ToList(), "ProductCategorieID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsCreate([Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products)
        {
            if (ModelState.IsValid)
            {

                if (products.SaleOff >= products.Price)
                {
                    ModelState.AddModelError("", @"Giá khuyến mãi lơn hơn giá bán. Bạn hãy nhập lại");
                }
                else
                {
                    _unitOfWork.ProductRepository.Insert(products);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thêm sản phẩm thành công !";
                    return RedirectToAction(nameof(ListProducts));
                }
            }
            ViewData["CollectionID"] = new SelectList(_unitOfWork.CollectionRepository.Get().ToList(), "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get().ToList(), "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        public IActionResult ProductsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = _unitOfWork.ProductRepository.GetByID(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CollectionID"] = new SelectList(_unitOfWork.CollectionRepository.Get().ToList(), "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get().ToList(), "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsEdit(int id, [Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products)
        {
            if (id != products.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ProductRepository.Update(products);
                    await _unitOfWork.Save();
                    TempData["result"] = "Chỉnh sửa sản phẩm thành công !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ListProducts));
            }
            ViewData["CollectionID"] = new SelectList(_unitOfWork.CollectionRepository.Get(), "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get(), "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ProductsDelete(int id)
        {
            if (id < 1)
                return false;
            var products = _unitOfWork.ProductRepository.GetByID(id);
            _unitOfWork.ProductRepository.Delete(products);
            await _unitOfWork.Save();
            TempData["result"] = "Xóa sản phẩm thành công !";

            if (products.Image != null)
            {
                string[] arr = products.Image.Split(',');

                foreach (var item in arr)
                {
                    DeleteImage(item);
                }
            }
            return true;
        }
        #endregion

        #region Size,Color
        public IActionResult ProductsSC(int? idsp)
        {
            var shopProductContext = _unitOfWork.ProductSCRepository.Get(p => p.ProductID == idsp, includeProperties: "Color,Size");
            ViewBag.idsp = idsp;
            return View(shopProductContext.ToList());
        }

        public IActionResult ProductsCreateSC(int? id)
        {
            ViewBag.IdSP = id;
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor");
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsCreateSCC([Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductSCRepository.Insert(productSizeColor);
                await _unitOfWork.Save();
                TempData["StatusMessage"] = "Tạo Thành Công";
                return RedirectToAction(nameof(ProductsSC), new { idsp = productSizeColor.ProductID });
            }
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        public IActionResult ProductsEditSC(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = _unitOfWork.ProductSCRepository.GetByID(id);
            if (productSizeColor == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsEditSC(int id, [Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (id != productSizeColor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ProductSCRepository.Update(productSizeColor);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
                return RedirectToAction(nameof(ProductsSC), new { idsp = productSizeColor.ProductID });
            }
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        [HttpPost, ActionName("ProductsDeleteSC")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ProductsDeleteSCConfirmed(int id)
        {
            if (id < 1)
                return false;
            var productSizeColor = _unitOfWork.ProductSCRepository.GetByID(id);
            _unitOfWork.ProductSCRepository.Delete(productSizeColor);
            await _unitOfWork.Save();
            TempData["StatusMessage"] = "Xóa Thành Công";
            return true;
        }

        #endregion

        #region ProductsSizeColors      

        public IActionResult ProductsSCIndex()
        {

            var query = new ProductSCViewModel()
            {
                ProductSizeColors = _unitOfWork.ProductSCRepository.Get(includeProperties: "Size,Color").ToList(),
                Products = _unitOfWork.ProductRepository.Get().ToList()
            };
            return View(query);
        }

        public IActionResult ProductsSCCreate()
        {
            ViewData["ProductID"] = new SelectList(_unitOfWork.ProductRepository.Get(), "ProductID", "Name");
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor");
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct");
            return View();
        }
        //

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsSCCreate([Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductSCRepository.Insert(productSizeColor);
                await _unitOfWork.Save();
                TempData["StatusMessage"] = "Tạo thành công";
                return RedirectToAction(nameof(ProductsSCIndex));
            }
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        public IActionResult ProductsSCEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = _unitOfWork.ProductSCRepository.GetByID(id);
            if (productSizeColor == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsSCEdit(int id, [Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (id != productSizeColor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ProductSCRepository.Update(productSizeColor);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ProductsSCIndex));
            }
            ViewData["ColorID"] = new SelectList(_unitOfWork.ColorRepository.Get(), "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_unitOfWork.SizeRepository.Get(), "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ProductsSCDelete(int id)
        {
            if (id < 1)
                return false;
            var productSizeColor = _unitOfWork.ProductSCRepository.GetByID(id);
            _unitOfWork.ProductSCRepository.Delete(productSizeColor);
            await _unitOfWork.Save();
            TempData["StatusMessage"] = "Thành công !";
            return true;
        }
        #endregion

        #region ProductsCategores

        public IActionResult ProductCategoriesCreate()
        {
            ViewData["ParentId"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get(), "ProductCategorieID", "Name");
            ViewBag.Productcato = _unitOfWork.ProductCategoryRepository.Get().ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCategoriesCreate([Bind("ProductCategorieID,Name,Image,CoverImage,Url,Soft,Active,Home,ParentId,TitleMeta,DescriptionMeta,Body")] ProductCategories productCategories)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductCategoryRepository.Insert(productCategories);
                await _unitOfWork.Save();
                TempData["result"] = "Thêm thành công ";
                return RedirectToAction(nameof(ProductCategoriesCreate));
            }
            ViewData["ParentId"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get(), "ProductCategorieID", "Name");
            ViewBag.Productcato = _unitOfWork.ProductCategoryRepository.Get().ToList();

            return View(productCategories);
        }

        public IActionResult ProductCategoriesEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_unitOfWork.ProductCategoryRepository.Get(), "ProductCategorieID", "Name");

            var productCategories = _unitOfWork.ProductCategoryRepository.GetByID(id);
            if (productCategories == null)
            {
                return NotFound();
            }
            return View(productCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCategoriesEdit(int id, [Bind("ProductCategorieID,Name,Image,CoverImage,Url,Soft,Active,Home,ParentId,TitleMeta,DescriptionMeta,Body")] ProductCategories productCategories)
        {
            if (id != productCategories.ProductCategorieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ProductCategoryRepository.Update(productCategories);
                    TempData["result"] = "Chỉnh sửa thành công ";

                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (productCategories.ProductCategorieID != id)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductCategoriesCreate));
            }
            return View(productCategories);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ProductCategoriesDelete(int id)
        {
            if (id < 1)
                return false;
            var productCategories = _unitOfWork.ProductCategoryRepository.GetByID(id);
            _unitOfWork.ProductCategoryRepository.Delete(productCategories);
            await _unitOfWork.Save();
            return true;
        }

        #endregion

        #region Color
        // GET: Colors
        public IActionResult ColorIndex()
        {
            return View(_unitOfWork.ColorRepository.Get().ToList());
        }
        public IActionResult ColorCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ColorCreate([Bind("ColorID,Code,NameColor")] Models.Color color)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ColorRepository.Insert(color);
                await _unitOfWork.Save();
                TempData["result"] = "Tạo thành công !";
                return RedirectToAction(nameof(ColorIndex));
            }
            return View(color);
        }

        public IActionResult ColorEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = _unitOfWork.ColorRepository.GetByID(id);
            if (color == null)
            {
                return NotFound();
            }
            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ColorEdit(int id, [Bind("ColorID,Code,NameColor")] Models.Color color)
        {
            if (id != color.ColorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ColorRepository.Update(color);
                    TempData["result"] = "Chỉnh sửa thành công !";
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ColorIndex));
            }
            return View(color);
        }


        [HttpPost, ActionName("ColorDelete")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ColorDeleteConfirmed(int id)
        {
            if (id < 1)
                return false;
            var color = _unitOfWork.ColorRepository.GetByID(id);
            _unitOfWork.ColorRepository.Delete(color);
            await _unitOfWork.Save();
            TempData["result"] = "Xóa thành công !";
            return true;
        }

        #endregion

        #region Size
        public IActionResult SizeIndex()
        {
            return View(_unitOfWork.SizeRepository.Get());
        }
        public IActionResult SizeCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SizeCreate([Bind("SizeID,SizeProduct")] Models.Size size)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SizeRepository.Insert(size);
                await _unitOfWork.Save();
                TempData["result"] = "Tạo thành công ";
                return RedirectToAction(nameof(SizeIndex));
            }
            return View(size);
        }

        public IActionResult SizeEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = _unitOfWork.SizeRepository.GetByID(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SizeEdit(int id, [Bind("SizeID,SizeProduct")] Models.Size size)
        {
            if (id != size.SizeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.SizeRepository.Update(size);
                    await _unitOfWork.Save();
                    TempData["result"] = "Chỉnh sửa thành công ";
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(SizeIndex));
            }
            return View(size);
        }


        [HttpPost, ActionName("SizeDelete")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> SizeDeleteConfirmed(int id)
        {
            if (id < 1)
                return false;
            var size = _unitOfWork.SizeRepository.GetByID(id);
            _unitOfWork.SizeRepository.Delete(size);
            await _unitOfWork.Save();
            TempData["result"] = "Xóa thành công ";
            return true;
        }
        #endregion
        //
        #region SpecialCategory (Tag)

        public ActionResult ListSpecialCategory()
        {

            return View(_unitOfWork.TagRepository.Get(a => a.Active, q => q.OrderBy(a => a.Soft)).ToList());
        }
        public IActionResult SpecialCategory()
        {
            var tags = _unitOfWork.TagRepository.Get(a => a.Active, q => q.OrderBy(a => a.Soft)).ToList();
            ViewBag.Tags = tags;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecialCategory([Bind("TagID,Name,Soft,Active")] Tags tags)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TagRepository.Insert(tags);
                await _unitOfWork.Save();
                TempData["result"] = "Thêm thành công ";
                return RedirectToAction(nameof(SpecialCategory));
            }
            return View(tags);
        }

        // GET: Tags/Edit/5
        public IActionResult SpecialCategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = _unitOfWork.TagRepository.GetByID(id);
            if (tags == null)
            {
                return NotFound();
            }
            return View(tags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecialCategoryEdit(int id, [Bind("TagID,Name,Soft,Active")] Tags tags)
        {
            if (id != tags.TagID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.TagRepository.Update(tags);
                    await _unitOfWork.Save();
                    TempData["result"] = "Chỉnh sửa thành công ";

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(SpecialCategory));
            }
            return View(tags);
        }


        // POST: Tags/Delete/5
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<bool> SpecialCategoryDelete(int id)
        {
            var tags = _unitOfWork.TagRepository.GetByID(id);
            _unitOfWork.TagRepository.Delete(tags);
            await _unitOfWork.Save();
            TempData["result"] = "Xóa thành công";
            return true;
        }

        #endregion
        //
        #region ProductsTags
        public IActionResult SpecialCategoryProducts()
        {
            ViewBag.Tp = _unitOfWork.TagsProductsRepository.Get(includeProperties: "Products,Tags").ToList();
            ViewData["ProductID"] = new SelectList(_unitOfWork.ProductRepository.Get().ToList(), "ProductID", "Name");
            ViewData["TagID"] = new SelectList(_unitOfWork.TagRepository.Get().ToList(), "TagID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecialCategoryProducts([Bind("TagID,ProductID")] TagProducts tagProducts)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TagsProductsRepository.Insert(tagProducts);
                await _unitOfWork.Save();
                TempData["result"] = "Tạo thành công !";
                return RedirectToAction(nameof(SpecialCategoryProducts));
            }
            ViewData["ProductID"] = new SelectList(_unitOfWork.ProductRepository.Get().ToList(), "ProductID", "Name");
            ViewData["TagID"] = new SelectList(_unitOfWork.TagRepository.Get().ToList(), "TagID", "Name");
            TempData["result"] = "Tạo thất bại !";
            return View(tagProducts);
        }

        public IActionResult SpecialCategoryProductsDelete(int? ido, int? idt)
        {
            if (ido == null || idt == null)
            {
                return NotFound();
            }
            ViewBag.id1 = ido;
            ViewBag.id2 = idt;
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SpecialCategoryProductsDeleteConfirmed(int id1, int id2)
        {
            if (id1 < 1 || id2 < 1)
            {
                TempData["result"] = "Thất bại!";
                return View();
            }
            var tagProducts = _unitOfWork.TagsProductsRepository.Get(a => a.TagID == id1 && a.ProductID == id2).FirstOrDefault();
            _unitOfWork.TagsProductsRepository.Delete(tagProducts);
            await _unitOfWork.Save();
            TempData["result"] = "Thành công !";
            return Redirect(nameof(SpecialCategoryProducts));
        }
        #endregion

        #region AticleCategories

        public IActionResult ArticleCategories()
        {
            ViewBag.atgory = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderByDescending(a => a.CategorySort));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleCategories([Bind("ArticleCategoryId,CategoryName,Url,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategorys articleCategories)
        {
            if (ModelState.IsValid)
            {
                SlugCovert slugcv = new SlugCovert();
                articleCategories.Slug = slugcv.UrlFriendly(articleCategories.CategoryName);
                _unitOfWork.ArticleCategoryRepository.Insert(articleCategories);
                await _unitOfWork.Save();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ArticleCategories));
            }
            return View(articleCategories);
        }
        public IActionResult ArticleCategoriesEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategories = _unitOfWork.ArticleCategoryRepository.GetByID(id);
            if (articleCategories == null)
            {
                return NotFound();
            }
            return View(articleCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleCategoriesEdit(int id, [Bind("ArticleCategoryId,CategoryName,Url,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategorys articleCategories)
        {
            if (id != articleCategories.ArticleCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ArticleCategoryRepository.Update(articleCategories);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ArticleCategories));
            }
            return View(articleCategories);
        }

        [HttpPost, ActionName("ArticleCategoriesDelete")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ArticleCategoriesDeleteConfirmed(int id)
        {
            if (id < 1)
                return false;
            var articleCategories = _unitOfWork.ArticleCategoryRepository.GetByID(id);
            _unitOfWork.ArticleCategoryRepository.Delete(articleCategories);
            await _unitOfWork.Save();
            TempData["result"] = "Thành công";
            return true;
        }

        #endregion

        #region Aticle

        public IActionResult ListAticle()
        {

            return View(_unitOfWork.ArticleRepository.Get().ToList());
        }

        public IActionResult AticleCreate()
        {
            ViewBag.list = _unitOfWork.ArticleRepository.Get().ToList();
            ViewData["ArticleCategorieId"] = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(), "ArticleCategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AticleCreate([Bind("Subject,Description,Body,Image,CreateDate,View,ArticleCategoryId,Active,Hot,Home,Url,TitleMeta,DescriptionMetaTitle,KeyWord")] Articles articles)
        {
            if (ModelState.IsValid)
            {
                articles.Hot = true;
                _unitOfWork.ArticleRepository.Insert(articles);
                await _unitOfWork.Save();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ListAticle));
            }
            ViewBag.list = _unitOfWork.ArticleRepository.Get().ToList();
            ViewData["ArticleCategorieId"] = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(), "ArticleCategoryId", "CategoryName");
            return View(articles);
        }

        public IActionResult ArticleEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = _unitOfWork.ArticleRepository.GetByID(id);
            ViewData["ArticleCategorieID"] = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(), "ArticleCategoryId", "CategoryName");

            if (articles == null)
            {
                return NotFound();
            }
            return View(articles);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ArticleEdit(int id, [Bind("Id,Subject,Description,Body,Image,CreateDate,View,ArticleCategoryId,Active,Hot,Home,Url,TitleMeta,DescriptionMetaTitle,KeyWord")] Articles articles)
        {
            if (id != articles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    articles.Hot = true;
                    _unitOfWork.ArticleRepository.Update(articles);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thành công";

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ListAticle));
            }
            ViewData["ArticleCategorieID"] = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(), "ArticleCategoryId", "CategoryName");

            return View(articles);
        }

        [HttpPost, ActionName("ArticleDelete")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> AticleDeleteConfirmed(int id)
        {
            if (id < 1)
                return false;
            var articles = _unitOfWork.ArticleRepository.GetByID(id);
            _unitOfWork.ArticleRepository.Delete(articles);
            await _unitOfWork.Save();
            TempData["result"] = "Thành công";
            return true;
        }

        #endregion

        #region Collection

        public IActionResult ListCollectionBST()
        {
            return View(_unitOfWork.CollectionRepository.Get(orderBy: q => q.OrderByDescending(a => a.Sort)));
        }

        public IActionResult ListCollection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListCollection([Bind("CollectionID,Name,Description,Image,Body,Quantity,Factory,Price,Sort,Hot,Home,Active,TitleMeta,Content,StatusProduct,BarCode,CreateDate,CreateBy")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CollectionRepository.Insert(collection);
                await _unitOfWork.Save();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ListCollectionBST));
            }
            return View(collection);
        }

        public IActionResult ListCollectionEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = _unitOfWork.CollectionRepository.GetByID(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListCollectionEdit(int id, [Bind("CollectionID,Name,Description,Image,Body,Quantity,Factory,Price,Sort,Hot,Home,Active,TitleMeta,Content,StatusProduct,BarCode,CreateDate,CreateBy")] Collection collection)
        {
            if (id != collection.CollectionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CollectionRepository.Update(collection);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thành công";

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ListCollectionBST));
            }
            return View(collection);
        }

        [HttpPost, ActionName("ListCollectionDelete")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ListCollectionDeleteha(int id)
        {
            if (id < 1)
            {
                TempData["result"] = "Lỗi";
                return false;
            }
            var collection = _unitOfWork.CollectionRepository.GetByID(id);
            _unitOfWork.CollectionRepository.Delete(collection);
            await _unitOfWork.Save();
            TempData["result"] = "Thành công !";
            return true;
        }

        #endregion

        #region Contacts

        public IActionResult ListContacts()
        {
            return View(_unitOfWork.ContactRepository.Get(orderBy: a => a.OrderBy(q => q.CreateDate)));
        }


        public IActionResult ListContactsCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListContactsCreate([Bind("ContactID,Fullname,Address,Mobile,Email,Subject,Body,CreateDate")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                contacts.CreateDate = DateTime.Now;
                _unitOfWork.ContactRepository.Insert(contacts);
                await _unitOfWork.Save();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ListContacts));
            }
            return View(contacts);
        }

        public IActionResult ListContactsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contacts = _unitOfWork.ContactRepository.GetByID(id);
            if (contacts == null)
            {
                return NotFound();
            }
            return View(contacts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListContactsEdit(int id, [Bind("ContactID,Fullname,Address,Mobile,Email,Subject,Body,CreateDate")] Contacts contacts)
        {
            if (id != contacts.ContactID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ContactRepository.Update(contacts);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thành công";

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ListContacts));
            }
            return View(contacts);
        }

        [HttpPost, ActionName("ListContactsDelete")]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ListContactsDelete(int id)
        {
            if (id < 1)
            {
                TempData["result"] = "Lỗi";
                return false;
            }
            var contacts = _unitOfWork.ContactRepository.GetByID(id);
            _unitOfWork.ContactRepository.Delete(contacts);
            await _unitOfWork.Save();
            TempData["result"] = "Thành công !";
            return true;
        }

        #endregion

        #region Configsite

        public IActionResult ListConfigsite()
        {
            return View(_unitOfWork.ConfigSiteRepository.Get().ToList());
        }

        public IActionResult ListConfigsiteCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListConfigsiteCreate([Bind("ConfigSiteID,Facebook,GooglePlus,Youtube,Linkedin,Twitter,GoogleAnalytics,LiveChat,GoogleMap,Title,Description,ContactInfo,FooterInfo,Hotline,Email,CoverImage,SaleOffProgram,nameShopee,urlWeb,FbMessage")] ConfigSites configSites)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.ConfigSiteRepository.Insert(configSites);
                await _unitOfWork.Save();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ListConfigsite));
            }
            return View(configSites);
        }

        public IActionResult ListConfigsiteEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configSites = _unitOfWork.ConfigSiteRepository.GetByID(id);
            if (configSites == null)
            {
                return NotFound();
            }
            return View(configSites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListConfigsiteEdit(int id, [Bind("ConfigSiteID,Facebook,GooglePlus,Youtube,Linkedin,Twitter,GoogleAnalytics,LiveChat,GoogleMap,Title,Description,ContactInfo,FooterInfo,Hotline,Email,CoverImage,SaleOffProgram,nameShopee,urlWeb,FbMessage")] ConfigSites configSites)
        {
            if (id != configSites.ConfigSiteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ConfigSiteRepository.Update(configSites);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ListConfigsite));
            }
            return View(configSites);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ListConfigsiteDelete(int id)
        {
            if (id < 1)
                return false;
            var configSites = _unitOfWork.ConfigSiteRepository.GetByID(id);
            _unitOfWork.ConfigSiteRepository.Delete(configSites);
            await _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Banner
        public IActionResult ListBanner()
        {
            return View(_unitOfWork.BannerRepository.Get(orderBy: a => a.OrderBy(q => q.GroupId)));
        }


        public IActionResult ListBannerCreate()
        {
            return View();
        }

        public void Resize(string h, int w, int he)
        {
            using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(h))
            {
                if (image.Width > w)
                {
                    image.Mutate(x => x.Resize(w, (image.Height / image.Width) * w));
                    image.Save(h);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListBannerCreate([Bind("BannerID,BannerName,CoverImage ,Width,Height,Active,GroupId,Url,Soft,Title,Content")] Banners banners)
        {
            if (ModelState.IsValid)
            {
                string h = "" + _hostingEnvironment.WebRootPath + "\\" + banners.CoverImage + "";
                Resize(h, banners.Width, banners.Height);
                _unitOfWork.BannerRepository.Insert(banners);
                await _unitOfWork.Save();
                TempData["result"] = "Thành công ";
                return RedirectToAction(nameof(ListBanner));
            }
            return View(banners);
        }
        public IActionResult ListBannerEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banners = _unitOfWork.BannerRepository.GetByID(id);
            if (banners == null)
            {
                return NotFound();
            }
            return View(banners);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListBannerEdit(int id, [Bind("BannerID,BannerName,CoverImage ,Width,Height,Active,GroupId,Url,Soft,Title,Content")] Banners banners)
        {
            if (id != banners.BannerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string h = "" + _hostingEnvironment.WebRootPath + "\\" + banners.CoverImage + "";
                    Resize(h, banners.Width, banners.Height);
                    _unitOfWork.BannerRepository.Update(banners);
                    await _unitOfWork.Save();
                    TempData["result"] = "Thành công ";

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ListBanner));
            }
            return View(banners);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<bool> ListBannerDelete(int id)
        {
            if (id < 1)
                return false;
            var banners = _unitOfWork.BannerRepository.GetByID(id);
            _unitOfWork.BannerRepository.Delete(banners);
            await _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Order
        public ActionResult ListOrder(int? page, string madonhang, string fromdate, string todate, string customerName, string customerEmail, string customerMobile, int status = -1, int payment = 0, int pageSize = 50)
        {
            int? pageNumber = page ?? 1;
            var orders = _unitOfWork.OrderRepository.Get(orderBy: q => q.OrderByDescending(a => a.Id), includeProperties: "OrderDetails");

            if (!string.IsNullOrEmpty(madonhang))
                orders = orders.Where(a => a.MaDonHang.Contains(madonhang));
            if (!string.IsNullOrEmpty(customerName))
                orders = orders.Where(a => a.Fullname.ToLower().Contains(customerName.ToLower()));
            if (!string.IsNullOrEmpty(customerEmail))
                orders = orders.Where(a => a.Email.ToLower().Contains(customerEmail.ToLower()));
            if (!string.IsNullOrEmpty(customerMobile))
                orders = orders.Where(a => a.Mobile.Contains(customerMobile));
            if (DateTime.TryParse(fromdate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var fd))
                orders = orders.Where(a => a.CreateDate.Date >= fd.Date);
            if (DateTime.TryParse(todate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var td))
                orders = orders.Where(a => a.CreateDate.Date <= td.Date);
            if (status >= 0)
                orders = orders.Where(a => a.Status == status);
            if (payment > 0)
                switch (payment)
                {
                    case 1:
                        orders = orders.Where(a => !a.Payment);
                        break;
                    case 2:
                        orders = orders.Where(a => a.Payment);
                        break;
                }

            var model = new ListOrderViewModel
            {
                Orders = PaginatedList<Order>.CreateAsync(orders, pageNumber ?? 1, pageSize),
                MaDonhang = madonhang,
                Status = status,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerMobile = customerMobile,
                FromDate = fromdate,
                ToDate = todate,
                PageSize = pageSize,
                Payment = payment
            };

            return View(model);
        }
        public ActionResult LoadOrder(int orderId = 0)
        {
            return ViewComponent("LoadOrder", new { orderId });
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public bool UpdateOrder(string notice, int payment = 0, int status = 0, int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetByID(orderId);
            if (order == null)
            {
                return false;
            }
            if (status >= 0)
            {
                order.Status = status;
            }
            if (payment > 0)
            {
                switch (payment)
                {
                    case 1:
                        order.Payment = false;
                        break;
                    case 2:
                        order.Payment = true;
                        break;
                }
            }

            _unitOfWork.SaveNotAync();
            return true;
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public bool UpdateOrderNotice(string notice, int thanhtoantruoc = 0, int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetByID(orderId);
            if (order == null)
            {
                return false;
            }
            order.ThanhToanTruoc = thanhtoantruoc;
            order.Body = notice;
            _unitOfWork.SaveNotAync();
            return true;
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public bool DeleteOrder(int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetByID(orderId);
            if (order == null)
            {
                return false;
            }
            order.Status = 3;
            _unitOfWork.SaveNotAync();
            return true;
        }
        public IActionResult ViewHoaDon(int orderId)
        {
            return ViewComponent("ViewHoaDon", new { orderId });
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public bool UpdateOrderInfo(string notice, string fullname, string address, string email, string mobile, string date, int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetByID(orderId);
            if (order == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(fullname))
            {
                order.Fullname = fullname;
            }
            if (!string.IsNullOrEmpty(address))
            {
                order.Address = address;
            }
            if (!string.IsNullOrEmpty(email))
            {
                order.Email = email;
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                order.Mobile = mobile;
            }
            if (DateTime.TryParse(date, new CultureInfo("vi-VN"), DateTimeStyles.None, out var td))
            {
                order.TransportDate = td;
            }
            order.Body = notice;
            _unitOfWork.SaveNotAync();
            return true;
        }
        public void ExportOrder()
        {
            var fileName = "danh-sach-don-hang-thang" + DateTime.Now.Month + "-nam-" + DateTime.Now.Year + ".xlsx";

            var orders = _unitOfWork.OrderRepository.Get(includeProperties: "OrderDetails").ToList();

            using var pck = new ExcelPackage();
            //Create the worksheet
            var ws = pck.Workbook.Worksheets.Add("Danh sách đơn hàng");
            ws.DefaultColWidth = 20;
            ws.DefaultRowHeight = 40;
            ws.Cells.Style.WrapText = true;
            ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Column(4).Width = 40;
            ws.Column(5).Width = 50;

            ws.Cells[1, 1].Value = "Mã ĐH";
            ws.Cells[1, 2].Value = "Tên";
            ws.Cells[1, 3].Value = "Điện thoại";
            ws.Cells[1, 4].Value = "Email";
            ws.Cells[1, 5].Value = "Địa chỉ";
            ws.Cells[1, 6].Value = "Tổng tiền";
            ws.Cells[1, 7].Value = "Đã thanh toán";
            ws.Cells[1, 8].Value = "Công nợ";
            ws.Cells[1, 9].Value = "Ngày đặt";
            ws.Cells[1, 10].Value = "Ngày giao";
            ws.Cells[1, 11].Value = "Vận chuyển";
            ws.Cells[1, 12].Value = "Tình trạng Thanh toán";
            ws.Cells[1, 13].Value = "Tình trạng đơn hàng";
            ws.Cells[1, 14].Value = "Ghi chú";
            var i = 2;
            foreach (var order in orders)
            {
                var status = "Đang xử lý";
                switch (order.Status)
                {
                    case 1:
                        status = "Đang giao hàng";
                        break;
                    case 2:
                        status = "Đã thanh toán";
                        break;
                    case 3:
                        status = "Hủy đơn";
                        break;
                }
                var transport = "Hình thức khác";
                switch (order.Transport)
                {
                    case 1:
                        status = "Đến địa chỉ người nhận";
                        break;
                    case 2:
                        status = "Khách đến nhận hàng";
                        break;
                    case 3:
                        status = "Qua bưu điện";
                        break;
                }

                var total = order.OrderDetails.Sum(a => a.Price * a.Quantity);
                var congno = total - order.ThanhToanTruoc;

                ws.Cells[i, 1].Value = order.MaDonHang;
                ws.Cells[i, 2].Value = order.Fullname;
                ws.Cells[i, 3].Value = order.Mobile;
                ws.Cells[i, 4].Value = order.Email;
                ws.Cells[i, 5].Value = order.Address;
                ws.Cells[i, 6].Value = total;
                ws.Cells[i, 7].Value = order.ThanhToanTruoc;
                ws.Cells[i, 8].Value = congno;
                ws.Cells[i, 9].Value = order.CreateDate.ToString("dd/MM/yyyy HH:mm");
                ws.Cells[i, 10].Value = order.TransportDate.ToString("dd/MM/yyyy");
                ws.Cells[i, 11].Value = transport;
                ws.Cells[i, 12].Value = !order.Payment ? "Chưa thanh toán" : "Đã thanh toán";
                ws.Cells[i, 13].Value = status;
                ws.Cells[i, 14].Value = order.Body;
                i++;
            }

            //Format the header for column 1-14
            using (var rng = ws.Cells["A1:N1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));  //Set color to dark blue
                rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            //Write it back to the client
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment;  filename=" + fileName + "");
            Response.Body.WriteAsync(pck.GetAsByteArray());
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}

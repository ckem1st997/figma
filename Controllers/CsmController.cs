﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using figma.Data;
using figma.Models;
using figma.OutFile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using figma.ViewModel;
using figma.CustomHandler;
using figma.DAL;

namespace figma.Controllers
{
    [Authorize(Roles = "Admin")]
    // giới hạn kích thước file
    //  [RequestFormLimits(MultipartBodyLengthLimit = 4096000)]
    //[Authorize(AuthenticationSchemes = AuthSchemes)]

    public class CsmController : Controller
    {
        // xác thực cả 2 loại
        // private const string AuthSchemes =
        //CookieAuthenticationDefaults.AuthenticationScheme + "," +
        //JwtBearerDefaults.AuthenticationScheme;

        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ShopProductContext _context;

        public CsmController(ShopProductContext context, IWebHostEnvironment hostEnvironment, UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }
        //
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> createImage1(List<IFormFile> filesadd)
        {
            DateTime dateTime = DateTime.Now;
            //  string createFolderDate = "" + dateTime.Year + "\\" + dateTime.Month + "\\" + dateTime.Day + "";
            string createFolderDate = DateTime.Now.ToString("yyyy/MM/dd");
            string path = _hostingEnvironment.WebRootPath + @"\uploads\" + createFolderDate + "";
            Console.WriteLine(path);
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("Path đã tồn tại !");
                }
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

            // copy file
            if (path == null)
                path = "image";

            if (filesadd == null || filesadd.Count == 0)
                return Ok(new { imgNode = "" });
            long size = filesadd.Sum(f => f.Length);
            var filePaths = new List<string>();
            string sql = "";
            foreach (var formFile in filesadd)
            {
                if (FormFileExtensions.IsImage(formFile))
                {
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "" + path + "");

                        filePaths.Add(filePath);
                        var randomname = DateTime.Now.ToFileTime() + Path.GetExtension(formFile.FileName);
                        var fileNameWithPath = string.Concat(filePath, "\\", randomname);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        if (sql.Length > 1)
                            sql = "" + sql + ",uploads/" + createFolderDate + "/" + randomname + "";
                        else
                            sql = "uploads/" + createFolderDate + "/" + randomname + "";
                        // sql = sql.Replace("\\", "/");

                    }
                }
                else
                    return Ok(new { imgNode = "" });
            }
            return Ok(new
            {
                imgNode = sql
            });
        }

        //

        [HttpPost]
        public async Task<IActionResult> createImage(List<IFormFile> filesadd)
        {
            DateTime dateTime = DateTime.Now;
            //  string createFolderDate = "" + dateTime.Year + "\\" + dateTime.Month + "\\" + dateTime.Day + "";
            string createFolderDate = DateTime.Now.ToString("yyyy/MM/dd");
            string path = _hostingEnvironment.WebRootPath + @"\uploads\" + createFolderDate + "";
            Console.WriteLine(path);
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("Path đã tồn tại !");
                }
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

            // copy file
            if (path == null)
                path = "image";

            if (filesadd == null || filesadd.Count == 0)
                return Ok(new { imgNode = "" });
            //   if(filesadd)
            long size = filesadd.Sum(f => f.Length);
            var filePaths = new List<string>();
            string sql = "";
            foreach (var formFile in filesadd)
            {

                if (FormFileExtensions.IsImage(formFile))
                {
                    if (formFile.Length > 0 && formFile.Length <= 4096000)
                    {
                        // full path to file in temp location
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "" + path + "");

                        filePaths.Add(filePath);
                        var randomname = DateTime.Now.ToFileTime() + Path.GetExtension(formFile.FileName);
                        var fileNameWithPath = string.Concat(filePath, "\\", randomname);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        // resize
                        using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(fileNameWithPath))
                        {
                            image.Mutate(x => x.Resize(image.Width > 720 ? 720 : image.Width, image.Height > 822 ? 822 : image.Height));
                            image.Save(fileNameWithPath);
                        }

                        if (sql.Length > 1)
                            sql = "" + sql + ",uploads/" + createFolderDate + "/" + randomname + "";
                        else
                            sql = "uploads/" + createFolderDate + "/" + randomname + "";
                        // sql = sql.Replace("\\", "/");

                    }
                    else
                        return Ok(new { imgNode = "" });

                }
                else
                    return Ok(new { imgNode = "" });
            }
            return Ok(new
            {
                imgNode = sql
            });
        }

        //delete file

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


        //
        public IActionResult ListProducts()
        {
            var shopProductContext = _unitOfWork.ProductRepository.Get(includeProperties: "Collection,ProductCategories");
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
                _unitOfWork.ProductRepository.Insert(products);
                await _unitOfWork.Save();
                TempData["result"] = "Thêm sản phẩm thành công !";
                return RedirectToAction(nameof(ListProducts));
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
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
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
            var query =
                from post in _unitOfWork.ProductSCRepository.Get()
                join meta in _unitOfWork.SizeRepository.Get() on post.SizeID equals meta.SizeID
                join meta1 in _unitOfWork.ColorRepository.Get() on post.ColorID equals meta1.ColorID
                join meta2 in _unitOfWork.ProductRepository.Get() on post.ProductID equals meta2.ProductID
                select new PSC { id = post.Id, name = meta2.Name, color = meta1.NameColor, size = meta.SizeProduct, image = meta2.Image };
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

        // bộ lọc chống giả mạo ajax
        // [AutoValidateAntiforgeryToken]
        //[HttpPost]
        //[IgnoreAntiforgeryToken]
        //public JsonResult AutoCompleteCity(string Prefix)
        //{
        //    //Searching records from list using LINQ query  
        //    var ProductsName = (from N in _context.Products
        //                        where N.Name.StartsWith(Prefix)
        //                        select new { N.Name }).Take(3);
        //    return Json(ProductsName);
        //}

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

        public async Task<IActionResult> ProductsSCEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
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

        // POST: Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            //  ViewBag.Tp = _context.TagProducts.Include(t => t.Products).Include(t => t.Tags);
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
        public async Task<IActionResult> ArticleCategories([Bind("ArticleCategoryId,CategoryName,Url,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategory articleCategories)
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
        public async Task<IActionResult> ArticleCategoriesEdit(int id, [Bind("ArticleCategoryId,CategoryName,Url,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategory articleCategories)
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
        public async Task<IActionResult> AticleCreate([Bind("Subject,Description,Body,Image,CreateDate,View,ArticleCategoryId,Active,Hot,Home,Url,TitleMeta,DescriptionMeta")] Article articles)
        {
            if (ModelState.IsValid)
            {
                //articles.View = 1;
                //articles.CreateDate = DateTime.Now;
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
        public async Task<IActionResult> ArticleEdit(int id, [Bind("Id,Subject,Description,Body,Image,CreateDate,View,ArticleCategoryId,Active,Hot,Home,Url,TitleMeta,DescriptionMeta")] Article articles)
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

        public async Task<IActionResult> ListCollectionBST()
        {
            return View(await _context.Collections.OrderBy(p => p.CollectionID).ToListAsync());
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
                _context.Add(collection);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ListCollectionBST));
            }
            return View(collection);
        }

        // GET: Collections/Edit/5
        public async Task<IActionResult> ListCollectionEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(collection);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Thành công";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.CollectionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListCollectionBST));
            }
            return View(collection);
        }
        //
        // POST: Collections/Delete/5
        public IActionResult ListCollectionDelete(int id)
        {
            if (id < 1)
            {
                TempData["result"] = "Lỗi";

                return Redirect(nameof(ListCollection));
            }
            var collection = _context.Collections.Find(id);
            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("ListCollectionDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListCollectionDeleteha(int id)
        {
            if (id < 1)
            {
                TempData["result"] = "Lỗi";
                return Redirect(nameof(ListCollection));
            }
            var collection = await _context.Collections.FindAsync(id);
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            TempData["result"] = "Thành công !";

            return RedirectToAction(nameof(ListCollection));
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.CollectionID == id);
        }
        #endregion


        #region Contacts

        // GET: Contacts
        public async Task<IActionResult> ListContacts()
        {
            return View(await _context.Contacts.OrderBy(a => a.ContactID).ToListAsync());
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
                _context.Add(contacts);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thành công";

                return RedirectToAction(nameof(ListContacts));
            }
            return View(contacts);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> ListContactsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return NotFound();
            }
            return View(contacts);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(contacts);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Thành công";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactsExists(contacts.ContactID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListContacts));
            }
            return View(contacts);
        }
        public IActionResult ListContactsDelete(int id)
        {
            if (id < 1)
            {
                TempData["result"] = "Lỗi";

                return Redirect(nameof(ListContacts));
            }
            var collection = _context.Contacts.Find(id);
            return View(collection);
        }

        [HttpPost, ActionName("ListContacts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListContactsDelete1111(int id)
        {
            if (id < 1)
            {
                TempData["result"] = "Lỗi";
                return Redirect(nameof(ListContacts));
            }
            var contacts = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contacts);
            await _context.SaveChangesAsync();
            TempData["result"] = "Thành công !";
            return Redirect(nameof(ListContacts));
        }

        private bool ContactsExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactID == id);
        }
        #endregion

        #region Configsite

        public async Task<IActionResult> ListConfigsite()
        {
            return View(await _context.ConfigSites.OrderBy(a => a.ConfigSiteID).ToListAsync());
        }

        // GET: ConfigSites/Create
        public IActionResult ListConfigsiteCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListConfigsiteCreate([Bind("ConfigSiteID,Facebook,GooglePlus,Youtube,Linkedin,Twitter,GoogleAnalytics,LiveChat,GoogleMap,Title,Description,ContactInfo,FooterInfo,Hotline,Email,CoverImage,SaleOffProgram")] ConfigSites configSites)
        {

            if (ModelState.IsValid)
            {


                _context.Add(configSites);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ListConfigsite));
            }
            return View(configSites);
        }

        // GET: ConfigSites/Edit/5
        public async Task<IActionResult> ListConfigsiteEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configSites = await _context.ConfigSites.FindAsync(id);
            if (configSites == null)
            {
                return NotFound();
            }
            return View(configSites);
        }

        // POST: ConfigSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListConfigsiteEdit(int id, [Bind("ConfigSiteID,Facebook,GooglePlus,Youtube,Linkedin,Twitter,GoogleAnalytics,LiveChat,GoogleMap,Title,Description,ContactInfo,FooterInfo,Hotline,Email,CoverImage,SaleOffProgram")] ConfigSites configSites)
        {
            if (id != configSites.ConfigSiteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(configSites);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfigSitesExists(configSites.ConfigSiteID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListConfigsite));
            }
            return View(configSites);
        }


        // POST: ConfigSites/Delete/5
        [HttpPost]
        public bool ListConfigsiteDelete(int id)
        {
            if (id < 1)
                return false;
            var configSites = _context.ConfigSites.Find(id);
            _context.ConfigSites.Remove(configSites);
            _context.SaveChanges();
            return true;
        }

        private bool ConfigSitesExists(int id)
        {
            return _context.ConfigSites.Any(e => e.ConfigSiteID == id);
        }


        #endregion

        #region Banner
        public async Task<IActionResult> ListBanner()
        {
            return View(await _context.Banners.OrderBy(a => a.BannerID).ToListAsync());
        }


        public IActionResult ListBannerCreate()
        {
            return View();
        }

        public void Resize(string h, int w, int he)
        {
            using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(h))
            {
                image.Mutate(x => x.Resize(w, he));
                image.Save(h);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListBannerCreate([Bind("BannerID,BannerName,CoverImage ,Width,Height,Active,GroupId,Url,Soft,Title,Content")] Banners banners)
        {
            if (ModelState.IsValid)
            {
                string h = "" + _hostingEnvironment.WebRootPath + "\\" + banners.CoverImage + "";
                switch (banners.GroupId)
                {
                    case 1:
                        Resize(h, 1440, 632);
                        break;

                    case 2:
                        Resize(h, 465, 330);
                        break;
                    case 3:
                        Resize(h, 770, 497);
                        break;
                    case 4:
                        Resize(h, 650, 243);
                        break;
                }
                _context.Add(banners);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thành công ";
                return RedirectToAction(nameof(ListBanner));
            }
            return View(banners);
        }

        // GET: Banners/Edit/5
        public async Task<IActionResult> ListBannerEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banners = await _context.Banners.FindAsync(id);
            if (banners == null)
            {
                return NotFound();
            }
            return View(banners);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    switch (banners.GroupId)
                    {
                        case 1:
                            Resize(h, 1440, 632);
                            break;

                        case 2:
                            Resize(h, 465, 330);
                            break;
                        case 3:
                            Resize(h, 770, 497);
                            break;
                        case 4:
                            Resize(h, 650, 243);
                            break;
                    }
                    _context.Update(banners);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Thành công ";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannersExists(banners.BannerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListBanner));
            }
            return View(banners);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public bool ListBannerDelete(int id)
        {
            if (id < 1)
                return false;
            var banners = _context.Banners.Find(id);
            _context.Banners.Remove(banners);
            _context.SaveChanges();
            return true;
        }

        private bool BannersExists(int id)
        {
            return _context.Banners.Any(e => e.BannerID == id);
        }
        #endregion


        #region Admin

        public class RegisterViewModel
        {

            [Required, MaxLength(50)]
            public string Username { get; set; }

            [Required, DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string Password { get; set; }

            [DataType(DataType.Password), Compare(nameof(Password)), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string ConfirmPassword { get; set; }
        }

        public class LoginViewModel
        {

            [Required, MaxLength(50)]
            public string Username { get; set; }

            [Required, DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
            public string Password { get; set; }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginCsm()
        {
            try
            {
                HttpContext.SignOutAsync(
CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception)
            {

                return View();

            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoginCsm([Bind] LoginViewModel user, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                if (ValidateAdmin(user.Username, user.Password))
                {
                    var users = _context.Admins.SingleOrDefault(a => a.Username == user.Username);
                    if (users != null)
                    {
                        var userClaims = new List<Claim>()
                {
                    new Claim("UserName", users.Username),
                    new Claim(ClaimTypes.Role, users.Role==null?"notAdmin":users.Role)
                 };
                        // add session
                        //foreach (var item in userClaims)
                        //{
                        //    HttpContext.Session.SetString(item.Type, item.Value);

                        //}
                        var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity), authProperties);
                        //    Execute("shoponline@gmail.com", "Shop1997", "hopxc1997@gmail.com", "Nguyễn Khả Hợp", "Thanh toán", "Đã mua").Wait();
                        //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        //   && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        //{
                        //    return Redirect(returnUrl);
                        //}
                        TempData["tq"] = "Thành công !";
                        return RedirectToAction("Index", "Csm");

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
                    return View();
                }
            }
            TempData["tq"] = "Thất bại !";

            return View(user);
        }

        [AllowAnonymous]
        public bool ValidateAdmin(string username, string password)
        {
            var admin = _context.Admins.SingleOrDefault(a => a.Username == username);
            return admin != null && new PasswordHasher<Admins>().VerifyHashedPassword(new Admins(), admin.Password, password) == PasswordVerificationResult.Success;
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
            //  returnUrl = returnUrl ?? Url.Content("~/");
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dk = _context.Admins.Where(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (dk != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashedPassword = new PasswordHasher<Admins>().HashPassword(new Admins(), model.Password);
                    await _context.Admins.AddAsync(new Admins { Username = model.Username, Password = hashedPassword, Role = "Admin", Active = true });
                    await _context.SaveChangesAsync();
                    TempData["tq"] = "Thành công";
                    return RedirectToAction("LoginCsm", "Csm");
                }
                ModelState.AddModelError("", "Lỗi đăng ký, xin vui lòng thử lại nha");

            }
            TempData["tq"] = "Thất bại";

            return View(model);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}

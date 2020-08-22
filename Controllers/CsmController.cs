using System;
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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace figma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CsmController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ShopProductContext _context;

        public CsmController(ShopProductContext context, IWebHostEnvironment hostEnvironment)
        {
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
        public IActionResult deleteImage(string filesadd)
        {
            var result = false;
            var h = filesadd;

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
                return Ok(new { result = false, h });
            return Ok(new
            {
                result
            });
        }

        #region Products


        //
        public async Task<IActionResult> ListProducts()
        {
            var shopProductContext = _context.Products.Include(p => p.Collection).Include(p => p.ProductCategories);
            return View(await shopProductContext.ToListAsync());
        }

        public IActionResult ProductsCreate()
        {
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name");
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsCreate([Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thêm sản phẩm thành công !";
                return RedirectToAction(nameof(ListProducts));
            }

            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        public async Task<IActionResult> ProductsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
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
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Chỉnh sửa sản phẩm thành công !";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListProducts));
            }
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        public async Task<IActionResult> ProductsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Collection)
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        [HttpPost, ActionName("ProductsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsDeleteConfirmed(int id, string listimage)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa sản phẩm thành công !";

            if (listimage != null)
            {
                string[] arr = listimage.Split(',');

                foreach (var item in arr)
                {
                    Console.WriteLine(item);
                    String filepath = Path.Combine(_hostingEnvironment.WebRootPath, item);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
            }
            return RedirectToAction(nameof(ListProducts));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
        #endregion

        #region Size,Color
        public async Task<IActionResult> ProductsSC(int? idsp)
        {
            var shopProductContext = _context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Where(p => p.ProductID == idsp);
            ViewBag.idsp = idsp;
            return View(await shopProductContext.ToListAsync());
        }

        public IActionResult ProductsCreateSC(int? id)
        {
            ViewBag.IdSP = id;
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsCreateSCC([Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSizeColor);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Tạo Thành Công";
                return RedirectToAction(nameof(ProductsSC), new { idsp = productSizeColor.ProductID });
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        public async Task<IActionResult> ProductsDetailsSC(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.NameSP = from a in _context.Products
                             where a.ProductID == id
                             select a.Name;
            ViewBag.Id = id;
            Console.WriteLine(2);
            Console.WriteLine(ViewBag.Id);
            var productSizeColor = await _context.ProductSizeColors
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }

            return View(productSizeColor);
        }

        public async Task<IActionResult> ProductsEditSC(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors.FirstOrDefaultAsync(m => m.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
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
                    _context.Update(productSizeColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeColorExists(productSizeColor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductsSC), new { idsp = productSizeColor.ProductID });
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        public async Task<IActionResult> ProductsDeleteSC(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }

            return View(productSizeColor);
        }

        [HttpPost, ActionName("ProductsDeleteSC")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsDeleteSCConfirmed(int id)
        {
            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
            _context.ProductSizeColors.Remove(productSizeColor);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Xóa Thành Công";
            return RedirectToAction(nameof(ProductsSC), new { idsp = productSizeColor.ProductID });
        }

        private bool ProductSizeColorExists(int id)
        {
            return _context.ProductSizeColors.Any(e => e.Id == id);
        }

        #endregion

        #region ProductsSizeColors       
        public async Task<IActionResult> ProductsSCIndex()
        {
            var shopProductContext = _context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size);
            return View(await shopProductContext.ToListAsync());
        }

        public IActionResult ProductsSCCreate()
        {
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsSCCreate([Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSizeColor);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Tạo thành công";
                return RedirectToAction(nameof(ProductsSCIndex));
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        // GET: ProductSizeColors/Edit/5
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
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
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
                    _context.Update(productSizeColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeColorExistsS(productSizeColor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductsSCIndex));
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }
        public async Task<IActionResult> ProductsSCDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }

            return View(productSizeColor);
        }

        [HttpPost, ActionName("ProductsSCDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
            _context.ProductSizeColors.Remove(productSizeColor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductsSCIndex));
        }

        private bool ProductSizeColorExistsS(int id)
        {
            return _context.ProductSizeColors.Any(e => e.Id == id);
        }
        #endregion

        #region ProductsCategores

        public async Task<IActionResult> ProductCategoriesCreate()
        {
            ViewData["ParentId"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");
            ViewBag.Productcato = await _context.ProductCategories.ToArrayAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCategoriesCreate([Bind("ProductCategorieID,Name,Image,CoverImage,Url,Soft,Active,Home,ParentId,TitleMeta,DescriptionMeta,Body")] ProductCategories productCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategories);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thêm thành công ";
                return RedirectToAction(nameof(ProductCategoriesCreate));
            }
            ViewData["ParentId"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");

            ViewBag.Productcato = await _context.ProductCategories.ToArrayAsync();

            return View(productCategories);
        }

        public async Task<IActionResult> ProductCategoriesEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");

            var productCategories = await _context.ProductCategories.FindAsync(id);
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
                    _context.Update(productCategories);
                    TempData["result"] = "Chỉnh sửa thành công ";

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoriesExistsS(productCategories.ProductCategorieID))
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
        public async Task<IActionResult> ProductCategoriesDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.ProductCategorieID == id);
            if (productCategories == null)
            {
                return NotFound();
            }

            return View(productCategories);
        }

        [HttpPost, ActionName("ProductCategoriesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCategoriesDeleteConfirmed(int id)
        {
            var productCategories = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategories);
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa thành công ";

            return RedirectToAction(nameof(ProductCategoriesCreate));
        }

        private bool ProductCategoriesExistsS(int id)
        {
            return _context.ProductCategories.Any(e => e.ProductCategorieID == id);
        }

        #endregion

        #region Color
        // GET: Colors
        public async Task<IActionResult> ColorIndex()
        {
            return View(await _context.Colors.ToListAsync());
        }

        // GET: Colors/Details/5
        public async Task<IActionResult> ColorDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorID == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // GET: Colors/Create
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
                _context.Add(color);
                await _context.SaveChangesAsync();
                TempData["result"] = "Tạo thành công !";
                return RedirectToAction(nameof(ColorIndex));
            }
            return View(color);
        }

        public async Task<IActionResult> ColorEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors.FindAsync(id);
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
                    _context.Update(color);
                    TempData["result"] = "Chỉnh sửa thành công !";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.ColorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ColorIndex));
            }
            return View(color);
        }

        public async Task<IActionResult> ColorDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorID == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        [HttpPost, ActionName("ColorDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ColorDeleteConfirmed(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa thành công !";
            return RedirectToAction(nameof(ColorIndex));
        }

        private bool ColorExists(int id)
        {
            return _context.Colors.Any(e => e.ColorID == id);
        }
        #endregion

        #region Size
        public async Task<IActionResult> SizeIndex()
        {
            return View(await _context.Sizes.ToListAsync());
        }

        public async Task<IActionResult> SizeDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeID == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
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
                _context.Add(size);
                await _context.SaveChangesAsync();
                TempData["result"] = "Tạo thành công ";
                return RedirectToAction(nameof(SizeIndex));
            }
            return View(size);
        }

        public async Task<IActionResult> SizeEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);
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
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Chỉnh sửa thành công ";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.SizeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SizeIndex));
            }
            return View(size);
        }

        public async Task<IActionResult> SizeDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeID == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        [HttpPost, ActionName("SizeDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SizeDeleteConfirmed(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa thành công ";
            return RedirectToAction(nameof(SizeIndex));
        }

        private bool SizeExists(int id)
        {
            return _context.Sizes.Any(e => e.SizeID == id);
        }
        #endregion

        #region SpecialCategory (Tag)

        public async Task<ActionResult> ListSpecialCategory()
        {

            return View(await _context.Tags.AsQueryable().OrderBy(a => a.Soft).ToListAsync());
        }
        public IActionResult SpecialCategory()
        {
            var tags = from a in _context.Tags
                       orderby a.Soft ascending
                       select a;
            ViewBag.Tags = tags;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecialCategory([Bind("TagID,Name,Soft,Active")] Tags tags)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tags);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thêm thành công ";
                return RedirectToAction(nameof(SpecialCategory));
            }
            return View(tags);
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> SpecialCategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _context.Tags.FindAsync(id);
            if (tags == null)
            {
                return NotFound();
            }
            return View(tags);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(tags);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Chỉnh sửa thành công ";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagsExists(tags.TagID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SpecialCategory));
            }
            return View(tags);
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> SpecialCategoryDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _context.Tags
                .FirstOrDefaultAsync(m => m.TagID == id);
            if (tags == null)
            {
                return NotFound();
            }

            return View(tags);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("SpecialCategoryDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed5(int id)
        {
            var tags = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tags);
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa thành công";

            return RedirectToAction(nameof(SpecialCategory));
        }

        private bool TagsExists(int id)
        {
            return _context.Tags.Any(e => e.TagID == id);
        }


        #endregion

        #region ProductsTags
        public IActionResult SpecialCategoryProducts()
        {
            ViewBag.Tp = _context.TagProducts.Include(t => t.Products).Include(t => t.Tags);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecialCategoryProducts([Bind("TagID,ProductID")] TagProducts tagProducts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tagProducts);
                await _context.SaveChangesAsync();
                TempData["result"] = "Tạo thành công !";
                return RedirectToAction(nameof(SpecialCategoryProducts));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", tagProducts.ProductID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name", tagProducts.TagID);
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

        // POST: TagProducts/Delete/5
        [HttpPost]
        public bool SpecialCategoryProductsDeleteConfirmed(int id1, int id2)
        {
            if (id1 < 1 || id2 < 1)
            {
                return false;
            }
            var tagProducts = _context.TagProducts.Where(a => a.TagID == id1 && a.ProductID == id2).FirstOrDefault();
            _context.TagProducts.Remove(tagProducts);
            _context.SaveChanges();
            //  TempData["result"] = "Thành công !";
            return true;
        }

        private bool TagProductsExists(int id)
        {
            return _context.TagProducts.Any(e => e.ProductID == id);
        }
        #endregion

        #region AticleCategories

        public IActionResult ArticleCategories()
        {
            ViewBag.atgory = from a in _context.ArticleCategories
                             orderby a.CategorySort ascending
                             select a;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleCategories([Bind("ArticleCategorieID,CategoryName,Link,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategories articleCategories)
        {
            if (ModelState.IsValid)
            {
                SlugCovert slugcv = new SlugCovert();
                articleCategories.Slug = slugcv.UrlFriendly(articleCategories.CategoryName);
                _context.Add(articleCategories);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thành công";
                return RedirectToAction(nameof(ArticleCategories));
            }
            return View(articleCategories);
        }
        public async Task<IActionResult> ArticleCategoriesEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategories = await _context.ArticleCategories.FindAsync(id);
            if (articleCategories == null)
            {
                return NotFound();
            }
            return View(articleCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleCategoriesEdit(int id, [Bind("ArticleCategorieID,CategoryName,Link,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategories articleCategories)
        {
            if (id != articleCategories.ArticleCategorieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleCategories);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleCategoriesExists(articleCategories.ArticleCategorieID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ArticleCategories));
            }
            return View(articleCategories);
        }
        public async Task<IActionResult> ArticleCategoriesDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategories = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.ArticleCategorieID == id);
            if (articleCategories == null)
            {
                return NotFound();
            }

            return View(articleCategories);
        }

        [HttpPost, ActionName("ArticleCategoriesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleCategoriesDeleteConfirmed(int id)
        {
            var articleCategories = await _context.ArticleCategories.FindAsync(id);
            _context.ArticleCategories.Remove(articleCategories);
            await _context.SaveChangesAsync();
            TempData["result"] = "Thành công";
            return RedirectToAction(nameof(ArticleCategories));
        }

        private bool ArticleCategoriesExists(int id)
        {
            return _context.ArticleCategories.Any(e => e.ArticleCategorieID == id);
        }

        #endregion

        #region Aticle

        public async Task<IActionResult> ListAticle()
        {

            return View(await _context.Articles.ToListAsync());
        }

        public IActionResult AticleCreate()
        {
            ViewBag.list = _context.Articles.ToList();
            ViewData["ArticleCategorieID"] = new SelectList(_context.ArticleCategories, "ArticleCategorieID", "CategoryName");

            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AticleCreate([Bind("ArticleID,Subject,Description,Body,Image,CreateDate,View,ArticleCategorieID,Active,Hot,Home,Url,TitleMeta,DescriptionMeta")] Articles articles)
        {
            if (ModelState.IsValid)
            {
                articles.View = 1;
                articles.CreateDate = DateTime.Now;
                articles.Hot = true;
                _context.Add(articles);

                await _context.SaveChangesAsync();
                TempData["result"] = "Thành công";

                return RedirectToAction(nameof(ListAticle));
            }
            ViewBag.list = _context.Articles.ToList();
            ViewData["ArticleCategorieID"] = new SelectList(_context.ArticleCategories, "ArticleCategorieID", "CategoryName");
            return View(articles);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> ArticleEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles.FindAsync(id);
            ViewData["ArticleCategorieID"] = new SelectList(_context.ArticleCategories, "ArticleCategorieID", "CategoryName");

            if (articles == null)
            {
                return NotFound();
            }
            return View(articles);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleEdit(int id, [Bind("ArticleID,Subject,Description,Body,Image,CreateDate,View,ArticleCategorieID,Active,Hot,Home,Url,TitleMeta,DescriptionMeta")] Articles articles)
        {
            if (id != articles.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    articles.Hot = true;
                    _context.Update(articles);
                    await _context.SaveChangesAsync();
                    TempData["result"] = "Thành công";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticlesExists(articles.ArticleID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListAticle));
            }
            ViewData["ArticleCategorieID"] = new SelectList(_context.ArticleCategories, "ArticleCategorieID", "CategoryName");

            return View(articles);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> ArticleDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (articles == null)
            {
                return NotFound();
            }

            return View(articles);
        }



        // POST: Articles/Delete/5
        [HttpPost, ActionName("ArticleDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AticleDeleteConfirmed(int id)
        {
            var articles = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(articles);
            await _context.SaveChangesAsync();
            TempData["result"] = "Thành công";

            return RedirectToAction(nameof(ListAticle));
        }

        private bool ArticlesExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
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

        // POST: Collections/Delete/5
        [HttpPost]
        public bool ListCollectionDelete(int id)
        {
            if (id < 1)
                return false;
            var collection = _context.Collections.Find(id);
            _context.Collections.Remove(collection);
            _context.SaveChanges();
            return true;
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


        [HttpPost]
        public bool ListContactsDelete(int id)
        {
            if (id < 1)
                return false;
            var contacts = _context.Contacts.Find(id);
            _context.Contacts.Remove(contacts);
            _context.SaveChanges();
            return true;
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
        public async Task<IActionResult> ListBannerCreate([Bind("BannerID,BannerName,CoverImage ,Width,Height,Active,GroupId,Url,Sort,Title,Content")] Banners banners)
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
        public async Task<IActionResult> ListBannerEdit(int id, [Bind("BannerID,BannerName,CoverImage ,Width,Height,Active,GroupId,Url,Sort,Title,Content")] Banners banners)
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

        [Route("/Csm")]
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




























    }
}

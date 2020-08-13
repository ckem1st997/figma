using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using figma.Data;
using figma.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using figma.OutFile;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace figma.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;


        private readonly ShopProductContext _context;

        public ProductsController(ShopProductContext context, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }



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
        public async Task<IActionResult> deleteImage(string filesadd)
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

        public async Task<IActionResult> Index()
        {
            var shopProductContext = _context.Products.Include(p => p.Collection).Include(p => p.ProductCategories);
            return View(await shopProductContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
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

        public IActionResult Create()
        {
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name");
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thêm sản phẩm thành công !";
                return RedirectToAction(nameof(Index));
            }

            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "CollectionID", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products)
        {
            if (id != products.ProductID)
            {
                TempData["result"] = "Lỗi thao tác, xin vui lòng thử lại !";
                return RedirectToAction(nameof(Index));

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "CollectionID", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        public async Task<IActionResult> Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string listimage)
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
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        //

        public async Task<IActionResult> IndexSC(int? idsp)
        {
            var shopProductContext = _context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Where(p => p.ProductID == idsp);
            return View(await shopProductContext.ToListAsync());
        }

        public IActionResult CreateSC(int? id)
        {
            Console.WriteLine(1);
            Console.WriteLine(id.ToString());
            ViewBag.IdSP = id;

            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSCC([Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSizeColor);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Tạo Thành Công";
                return RedirectToAction(nameof(IndexSC), new { idsp = productSizeColor.ProductID });
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }


        // Size and Color
        public async Task<IActionResult> DetailsSC(int? id)
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

        public async Task<IActionResult> EditSC(int? id)
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
            //  ViewBag.id = from a in _context.;
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSC(int id, [Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
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
                return RedirectToAction(nameof(IndexSC), new { idsp = productSizeColor.ProductID });
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        public async Task<IActionResult> DeleteSC(int? id)
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

        [HttpPost, ActionName("DeleteSC")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSCConfirmed(int id)
        {
            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
            _context.ProductSizeColors.Remove(productSizeColor);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Xóa Thành Công";
            return RedirectToAction(nameof(IndexSC), new { idsp = productSizeColor.ProductID });
        }

        private bool ProductSizeColorExists(int id)
        {
            return _context.ProductSizeColors.Any(e => e.Id == id);
        }

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DetailsSCConfirmed(int id, string listimage)
        //{
        //    var products = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(products);
        //    await _context.SaveChangesAsync();
        //    if (listimage != null)
        //    {
        //        string[] arr = listimage.Split(',');

        //        foreach (var item in arr)
        //        {
        //            Console.WriteLine(item);
        //            //   item = item.Replace("", "");
        //            String filepath = Path.Combine(_hostingEnvironment.WebRootPath, item);
        //            if (System.IO.File.Exists(filepath))
        //            {
        //                System.IO.File.Delete(filepath);
        //                ///  Console.WriteLine("999");
        //            }
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

    }
}









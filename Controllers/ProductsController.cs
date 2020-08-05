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

        // viet ajax them hinh anh vao thư mục temp
        // xong trả về đường dẫn, thêm app img với src trả về
        [HttpPost]
        public async Task<IActionResult> createImage(List<IFormFile> filesadd)
        {
            DateTime dateTime = DateTime.Now;
            //test and create folder
            string createFolderDate = "" + dateTime.Year + "\\" + dateTime.Month + "\\" + dateTime.Day + "";
            string path = _hostingEnvironment.WebRootPath + @"\" + createFolderDate + "";
            Console.WriteLine(path);
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("Path đã tồn tại !");
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
                //  di.Delete();
                //    Console.WriteLine("The directory was deleted successfully.");
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
                return Content("file not selected");
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

                        var fileNameWithPath = string.Concat(filePath, "\\", formFile.FileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(fileNameWithPath))
                        {
                            image.Mutate(x => x.Resize(image.Width > 720 ? 720 : image.Width, image.Height > 822 ? 822 : image.Height));
                            image.Save(fileNameWithPath);
                        }

                        if (sql.Length > 1)
                            sql = "" + sql + "," + createFolderDate + "/" + formFile.FileName + "";
                        else
                            sql = "" + createFolderDate + "/" + formFile.FileName + "";
                        sql = sql.Replace("\\", "/");

                    }
                }
                else
                    return Ok(new { formFile.ContentType, r = false });
            }
            return Ok(new
            {
                sql
            });
        }

        // code sao chép hình ảnh vào thư mục image

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var shopProductContext = _context.Products.Include(p => p.Collection).Include(p => p.ProductCategories);
            return View(await shopProductContext.ToListAsync());
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "Name");
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products, List<IFormFile> files)
        {

            foreach (var item in files)
            {
                if (!FormFileExtensions.IsImage(item))
                    return Content("File không phải là hình ảnh");
            }
            if (ModelState.IsValid)
            {

                DateTime dateTime = DateTime.Now;
                //test and create folder
                string createFolderDate = "" + dateTime.Year + "\\" + dateTime.Month + "\\" + dateTime.Day + "";
                string path = _hostingEnvironment.WebRootPath + @"\" + createFolderDate + "";
                Console.WriteLine(path);
                try
                {
                    // Determine whether the directory exists.
                    if (Directory.Exists(path))
                    {
                        Console.WriteLine("Path đã tồn tại !");
                    }

                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                    // Delete the directory.
                    //  di.Delete();
                    //    Console.WriteLine("The directory was deleted successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                finally { }

                // copy file
                if (path == null)
                    path = "image";

                if (files == null || files.Count == 0)
                    return Content("file not selected");
                long size = files.Sum(f => f.Length);
                var filePaths = new List<string>();
                string sql = "";
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "" + path + "");

                        filePaths.Add(filePath);

                        var fileNameWithPath = string.Concat(filePath, "\\", formFile.FileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(fileNameWithPath))
                        {
                            //822
                            image.Mutate(x => x
                                 .Resize(image.Width > 720 ? 720 : image.Width, image.Height > 822 ? 822 : image.Height));
                            image.Save(fileNameWithPath);
                        }

                        if (sql.Length > 1)
                            sql = "" + sql + "," + createFolderDate + "/" + formFile.FileName + "";
                        else
                            sql = "" + createFolderDate + "/" + formFile.FileName + "";
                    }
                }

                products.Image = sql.Replace("\\", "/");
                _context.Add(products);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thêm sản phẩm thành công !";
                return RedirectToAction(nameof(Index));
            }

            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "CollectionID", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        // GET: Products/Edit/5
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
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "CollectionID", products.CollectionID);
            ViewData["ProductCategorieID"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name", products.ProductCategorieID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Description,Image,Body,ProductCategorieID,Quantity,Factory,Price,SaleOff,QuyCach,Sort,Hot,Home,Active,TitleMeta,DescriptionMeta,GiftInfo,Content,StatusProduct,CollectionID,BarCode,CreateDate,CreateBy")] Products products, string deletefile)
        {
            if (id != products.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (deletefile != null)
                {
                    string[] arr = deletefile.Split(',');

                    foreach (var item in arr)
                    {
                        //   item = item.Replace("", "");
                        String filepath = Path.Combine(_hostingEnvironment.WebRootPath, item);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                            ///  Console.WriteLine("999");

                        }
                    }
                }
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
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

        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string listimage)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            if (listimage != null)
            {
                string[] arr = listimage.Split(',');

                foreach (var item in arr)
                {
                    Console.WriteLine(item);
                    //   item = item.Replace("", "");
                    String filepath = Path.Combine(_hostingEnvironment.WebRootPath, item);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                        ///  Console.WriteLine("999");
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        // Size and Color



        public async Task<IActionResult> DetailsSC(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsSCConfirmed(int id, string listimage)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            if (listimage != null)
            {
                string[] arr = listimage.Split(',');

                foreach (var item in arr)
                {
                    Console.WriteLine(item);
                    //   item = item.Replace("", "");
                    String filepath = Path.Combine(_hostingEnvironment.WebRootPath, item);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                        ///  Console.WriteLine("999");
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}









using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using figma.Data;
using figma.Models;
using figma.OutFile;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace figma.Controllers
{
    public class CsmController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ShopProductContext _context;

        public CsmController(ShopProductContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Details(int? id)
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


        #region Products

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

            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "CollectionID", products.CollectionID);
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
            ViewData["CollectionID"] = new SelectList(_context.Collections, "CollectionID", "CollectionID", products.CollectionID);
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
        public async Task<IActionResult> ColorCreate([Bind("ColorID,Code,NameColor")] Color color)
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
        public async Task<IActionResult> ColorEdit(int id, [Bind("ColorID,Code,NameColor")] Color color)
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
        public async Task<IActionResult> SizeCreate([Bind("SizeID,SizeProduct")] Size size)
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
        public async Task<IActionResult> SizeEdit(int id, [Bind("SizeID,SizeProduct")] Size size)
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

        //public async Task<IActionResult> SpecialCategoryProductsEdit(int? ido, int? idt)
        //{
        //    if (ido == null||idt==null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Description", idt);
        //    ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name", ido);
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SpecialCategoryProductsEdit(int? ido, int? idt, [Bind("TagID,ProductID")] TagProducts tagProducts)
        //{
        //    if (id != tagProducts.ProductID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tagProducts);
        //            await _context.SaveChangesAsync();
        //            TempData["result"] = "Thành công !";
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TagProductsExists(tagProducts.ProductID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(SpecialCategoryProducts));
        //    }
        //    ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", tagProducts.ProductID);
        //    ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name", tagProducts.TagID);
        //    return View(tagProducts);
        //}

        // GET: TagProducts/Delete/5
        public async Task<IActionResult> SpecialCategoryProductsDelete(int? ido, int? idt)
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
            if (id1 == null || id2 == null)
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleCategories([Bind("ArticleCategorieID,CategoryName,Link,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategories articleCategories)
        {
            if (ModelState.IsValid)
            {
                SlugCovert slug = new SlugCovert();
                articleCategories.Slug =slug.GenerateSlug(articleCategories.CategoryName);
                _context.Add(articleCategories);
                await _context.SaveChangesAsync();
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
            return RedirectToAction(nameof(ArticleCategories));
        }

        private bool ArticleCategoriesExists(int id)
        {
            return _context.ArticleCategories.Any(e => e.ArticleCategorieID == id);
        }

        #endregion




































    }
}

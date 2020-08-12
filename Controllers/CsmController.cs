using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Data;
using figma.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace figma.Controllers
{
    public class CsmController : Controller
    {


        private readonly ShopProductContext _context;

        public CsmController(ShopProductContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.ProductCategories, "ProductCategorieID", "Name");
            ViewBag.Productcato = _context.ProductCategories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCategorieID,Name,Image,CoverImage,Url,Soft,Active,Home,ParentId,TitleMeta,DescriptionMeta,Body")] ProductCategories productCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategories);
                await _context.SaveChangesAsync();
                TempData["result"] = "Thêm thành công ";
                return RedirectToAction(nameof(Create));
            }
            TempData["result"] = "Lỗi, xin bạn vui lòng thử lại";

            return View(productCategories);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategories.FindAsync(id);
            if (productCategories == null)
            {
                return NotFound();
            }
            return View(productCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductCategorieID,Name,Image,CoverImage,Url,Soft,Active,Home,ParentId,TitleMeta,DescriptionMeta,Body")] ProductCategories productCategories)
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
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoriesExists(productCategories.ProductCategorieID))
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
            return View(productCategories);
        }

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategories = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoriesExists(int id)
        {
            return _context.ProductCategories.Any(e => e.ProductCategorieID == id);
        }

        // Colletion
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
                return RedirectToAction(nameof(Index));
            }
            return View(collection);

        }



    }
}

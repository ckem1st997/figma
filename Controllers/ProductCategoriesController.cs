using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using figma.Data;
using figma.Models;

namespace figma.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ShopProductContext _context;

        public ProductCategoriesController(ShopProductContext context)
        {
            _context = context;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCategories.ToListAsync());
        }

        // GET: ProductCategories/Details/5
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

        // GET: ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCategorieID,Name,Image,CoverImage,Url,Soft,Active,Home,ParentId,TitleMeta,DescriptionMeta,Body")] ProductCategories productCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(productCategories);
        }

        // GET: ProductCategories/Edit/5
        [Route("Csm/Edit/{id}")]

        public async Task<IActionResult> Edit(int? id)
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

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Csm/Edit/{id}")]

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
                    TempData["result"] = "Chỉnh sửa thành công ";

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
                return RedirectToAction(nameof(Create), "Csm");
            }
            return View(productCategories);
        }

     [Route("Csm/Delete/{id}")]
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

        // POST: ProductCategories/Delete/5
        [HttpPost]
        [Route("Csm/Delete/{id}")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategories = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategories);
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa thành công ";

            return RedirectToAction(nameof(Create),"Csm");
        }

        private bool ProductCategoriesExists(int id)
        {
            return _context.ProductCategories.Any(e => e.ProductCategorieID == id);
        }
    }
}

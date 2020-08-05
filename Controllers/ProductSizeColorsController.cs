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
    public class ProductSizeColorsController : Controller
    {
        private readonly ShopProductContext _context;

        public ProductSizeColorsController(ShopProductContext context)
        {
            _context = context;
        }

        // GET: ProductSizeColors
        public async Task<IActionResult> Index()
        {
            var shopProductContext = _context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size);
            return View(await shopProductContext.ToListAsync());
        }

        // GET: ProductSizeColors/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ProductSizeColors/Create
        public IActionResult Create()
        {
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct");
            return View();
        }

        // POST: ProductSizeColors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSizeColor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "NameColor", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeProduct", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        // GET: ProductSizeColors/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        // POST: ProductSizeColors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductID,ColorID,SizeID")] ProductSizeColor productSizeColor)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", productSizeColor.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", productSizeColor.SizeID);
            return View(productSizeColor);
        }

        // GET: ProductSizeColors/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ProductSizeColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
            _context.ProductSizeColors.Remove(productSizeColor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeColorExists(int id)
        {
            return _context.ProductSizeColors.Any(e => e.Id == id);
        }
    }
}

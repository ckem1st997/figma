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
    public class TagProductsController : Controller
    {
        private readonly ShopProductContext _context;

        public TagProductsController(ShopProductContext context)
        {
            _context = context;
        }

        // GET: TagProducts
        public async Task<IActionResult> Index()
        {
            var shopProductContext = _context.TagProducts.Include(t => t.Products).Include(t => t.Tags);
            return View(await shopProductContext.ToListAsync());
        }

        // GET: TagProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagProducts = await _context.TagProducts
                .Include(t => t.Products)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (tagProducts == null)
            {
                return NotFound();
            }

            return View(tagProducts);
        }

        // GET: TagProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Description");
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name");
            return View();
        }

        // POST: TagProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagID,ProductID")] TagProducts tagProducts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tagProducts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Description", tagProducts.ProductID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name", tagProducts.TagID);
            return View(tagProducts);
        }

        // GET: TagProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagProducts = await _context.TagProducts.FindAsync(id);
            if (tagProducts == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Description", tagProducts.ProductID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name", tagProducts.TagID);
            return View(tagProducts);
        }

        // POST: TagProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagID,ProductID")] TagProducts tagProducts)
        {
            if (id != tagProducts.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tagProducts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagProductsExists(tagProducts.ProductID))
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
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Description", tagProducts.ProductID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "Name", tagProducts.TagID);
            return View(tagProducts);
        }

        // GET: TagProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagProducts = await _context.TagProducts
                .Include(t => t.Products)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (tagProducts == null)
            {
                return NotFound();
            }

            return View(tagProducts);
        }

        // POST: TagProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tagProducts = await _context.TagProducts.FindAsync(id);
            _context.TagProducts.Remove(tagProducts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagProductsExists(int id)
        {
            return _context.TagProducts.Any(e => e.ProductID == id);
        }
    }
}

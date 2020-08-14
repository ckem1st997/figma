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
    public class ArticleCategoriesController : Controller
    {
        private readonly ShopProductContext _context;

        public ArticleCategoriesController(ShopProductContext context)
        {
            _context = context;
        }

        // GET: ArticleCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticleCategories.ToListAsync());
        }

        // GET: ArticleCategories/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ArticleCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArticleCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleCategorieID,CategoryName,Link,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategories articleCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articleCategories);
        }

        // GET: ArticleCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: ArticleCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleCategorieID,CategoryName,Link,CategorySort,CategoryActive,ParentId,ShowHome,ShowMenu,Slug,Hot,TitleMeta,DescriptionMeta")] ArticleCategories articleCategories)
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
                return RedirectToAction(nameof(Index));
            }
            return View(articleCategories);
        }

        // GET: ArticleCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ArticleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleCategories = await _context.ArticleCategories.FindAsync(id);
            _context.ArticleCategories.Remove(articleCategories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleCategoriesExists(int id)
        {
            return _context.ArticleCategories.Any(e => e.ArticleCategorieID == id);
        }
    }
}

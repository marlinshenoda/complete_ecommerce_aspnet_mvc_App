using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eTickets.Core.Entities;
using eTickets.Data;

namespace eTickets.Web.Controllers
{
    public class ShoppingCardItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCardItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCardItems
        public async Task<IActionResult> Index()
        {
              return _context.ShoppingCartItems != null ? 
                          View(await _context.ShoppingCartItems.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ShoppingCartItems'  is null.");
        }

        // GET: ShoppingCardItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingCartItems == null)
            {
                return NotFound();
            }

            var shoppingCardItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCardItem == null)
            {
                return NotFound();
            }

            return View(shoppingCardItem);
        }

        // GET: ShoppingCardItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCardItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,ShoppingCartId")] ShoppingCardItem shoppingCardItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCardItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCardItem);
        }

        // GET: ShoppingCardItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingCartItems == null)
            {
                return NotFound();
            }

            var shoppingCardItem = await _context.ShoppingCartItems.FindAsync(id);
            if (shoppingCardItem == null)
            {
                return NotFound();
            }
            return View(shoppingCardItem);
        }

        // POST: ShoppingCardItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,ShoppingCartId")] ShoppingCardItem shoppingCardItem)
        {
            if (id != shoppingCardItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCardItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCardItemExists(shoppingCardItem.Id))
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
            return View(shoppingCardItem);
        }

        // GET: ShoppingCardItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingCartItems == null)
            {
                return NotFound();
            }

            var shoppingCardItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCardItem == null)
            {
                return NotFound();
            }

            return View(shoppingCardItem);
        }

        // POST: ShoppingCardItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingCartItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShoppingCartItems'  is null.");
            }
            var shoppingCardItem = await _context.ShoppingCartItems.FindAsync(id);
            if (shoppingCardItem != null)
            {
                _context.ShoppingCartItems.Remove(shoppingCardItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCardItemExists(int id)
        {
          return (_context.ShoppingCartItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

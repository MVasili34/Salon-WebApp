using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWebApp.Data;
using SalonWebApp.Models;

namespace SalonWebApp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClientManagment
        public async Task<IActionResult> Index()
        {
              return _context.ClientManagment != null ? 
                          View(await _context.ClientManagment.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ClientManagment'  is null.");
        }

        // GET: ClientManagment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClientManagment == null)
            {
                return NotFound();
            }

            var clientAdministrate = await _context.ClientManagment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clientAdministrate == null)
            {
                return NotFound();
            }

            return View(clientAdministrate);
        }

        // GET: ClientManagment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClientManagment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FullName,DateOfBirth,Telephone,Email")] ClientAdministrate clientAdministrate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientAdministrate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientAdministrate);
        }

        // GET: ClientManagment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClientManagment == null)
            {
                return NotFound();
            }

            var clientAdministrate = await _context.ClientManagment.FindAsync(id);
            if (clientAdministrate == null)
            {
                return NotFound();
            }
            return View(clientAdministrate);
        }

        // POST: ClientManagment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,DateOfBirth,Telephone,Email")] ClientAdministrate clientAdministrate)
        {
            if (id != clientAdministrate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientAdministrate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientAdministrateExists(clientAdministrate.ID))
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
            return View(clientAdministrate);
        }

        // GET: ClientManagment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClientManagment == null)
            {
                return NotFound();
            }

            var clientAdministrate = await _context.ClientManagment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clientAdministrate == null)
            {
                return NotFound();
            }

            return View(clientAdministrate);
        }

        // POST: ClientManagment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClientManagment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ClientManagment'  is null.");
            }
            var clientAdministrate = await _context.ClientManagment.FindAsync(id);
            if (clientAdministrate != null)
            {
                _context.ClientManagment.Remove(clientAdministrate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientAdministrateExists(int id)
        {
          return (_context.ClientManagment?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

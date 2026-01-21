using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcoTrack.Data;
using EcoTrack.Models;

namespace EcoTrack.Controllers
{
    public class EmissionSourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmissionSourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmissionSources
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmissionSources.Include(e => e.Organization);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmissionSources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSource = await _context.EmissionSources
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emissionSource == null)
            {
                return NotFound();
            }

            return View(emissionSource);
        }

        // GET: EmissionSources/Create
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Name");
            return View();
        }

        // POST: EmissionSources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Description,OrganizationId")] EmissionSource emissionSource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emissionSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Name", emissionSource.OrganizationId);
            return View(emissionSource);
        }

        // GET: EmissionSources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSource = await _context.EmissionSources.FindAsync(id);
            if (emissionSource == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Name", emissionSource.OrganizationId);
            return View(emissionSource);
        }

        // POST: EmissionSources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Description,OrganizationId")] EmissionSource emissionSource)
        {
            if (id != emissionSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emissionSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmissionSourceExists(emissionSource.Id))
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
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Name", emissionSource.OrganizationId);
            return View(emissionSource);
        }

        // GET: EmissionSources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSource = await _context.EmissionSources
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emissionSource == null)
            {
                return NotFound();
            }

            return View(emissionSource);
        }

        // POST: EmissionSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emissionSource = await _context.EmissionSources.FindAsync(id);
            if (emissionSource != null)
            {
                _context.EmissionSources.Remove(emissionSource);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmissionSourceExists(int id)
        {
            return _context.EmissionSources.Any(e => e.Id == id);
        }
    }
}

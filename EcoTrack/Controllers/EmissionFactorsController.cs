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
    public class EmissionFactorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmissionFactorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmissionFactors
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmissionFactors.ToListAsync());
        }

        // GET: EmissionFactors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionFactor = await _context.EmissionFactors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emissionFactor == null)
            {
                return NotFound();
            }

            return View(emissionFactor);
        }

        // GET: EmissionFactors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmissionFactors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Unit,Co2EquivalentPerUnit,Year,IsActive")] EmissionFactor emissionFactor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emissionFactor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emissionFactor);
        }

        // GET: EmissionFactors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionFactor = await _context.EmissionFactors.FindAsync(id);
            if (emissionFactor == null)
            {
                return NotFound();
            }
            return View(emissionFactor);
        }

        // POST: EmissionFactors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Unit,Co2EquivalentPerUnit,Year,IsActive")] EmissionFactor emissionFactor)
        {
            if (id != emissionFactor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emissionFactor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmissionFactorExists(emissionFactor.Id))
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
            return View(emissionFactor);
        }

        // GET: EmissionFactors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionFactor = await _context.EmissionFactors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emissionFactor == null)
            {
                return NotFound();
            }

            return View(emissionFactor);
        }

        // POST: EmissionFactors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emissionFactor = await _context.EmissionFactors.FindAsync(id);
            if (emissionFactor != null)
            {
                _context.EmissionFactors.Remove(emissionFactor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmissionFactorExists(int id)
        {
            return _context.EmissionFactors.Any(e => e.Id == id);
        }
    }
}

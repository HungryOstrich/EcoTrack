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
    public class ActivityLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ActivityLogs.Include(a => a.EmissionFactor).Include(a => a.EmissionSource);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActivityLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs
                .Include(a => a.EmissionFactor)
                .Include(a => a.EmissionSource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityLog == null)
            {
                return NotFound();
            }

            return View(activityLog);
        }

        // GET: ActivityLogs/Create
        public IActionResult Create()
        {
            ViewData["EmissionFactorId"] = new SelectList(_context.EmissionFactors, "Id", "Name");
            ViewData["EmissionSourceId"] = new SelectList(_context.EmissionSources, "Id", "Name");
            return View();
        }

        // POST: ActivityLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Quantity,CalculatedCo2Emission,EmissionSourceId,EmissionFactorId")] ActivityLog activityLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmissionFactorId"] = new SelectList(_context.EmissionFactors, "Id", "Name", activityLog.EmissionFactorId);
            ViewData["EmissionSourceId"] = new SelectList(_context.EmissionSources, "Id", "Name", activityLog.EmissionSourceId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog == null)
            {
                return NotFound();
            }
            ViewData["EmissionFactorId"] = new SelectList(_context.EmissionFactors, "Id", "Name", activityLog.EmissionFactorId);
            ViewData["EmissionSourceId"] = new SelectList(_context.EmissionSources, "Id", "Name", activityLog.EmissionSourceId);
            return View(activityLog);
        }

        // POST: ActivityLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Quantity,CalculatedCo2Emission,EmissionSourceId,EmissionFactorId")] ActivityLog activityLog)
        {
            if (id != activityLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityLogExists(activityLog.Id))
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
            ViewData["EmissionFactorId"] = new SelectList(_context.EmissionFactors, "Id", "Name", activityLog.EmissionFactorId);
            ViewData["EmissionSourceId"] = new SelectList(_context.EmissionSources, "Id", "Name", activityLog.EmissionSourceId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs
                .Include(a => a.EmissionFactor)
                .Include(a => a.EmissionSource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityLog == null)
            {
                return NotFound();
            }

            return View(activityLog);
        }

        // POST: ActivityLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog != null)
            {
                _context.ActivityLogs.Remove(activityLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityLogExists(int id)
        {
            return _context.ActivityLogs.Any(e => e.Id == id);
        }
    }
}

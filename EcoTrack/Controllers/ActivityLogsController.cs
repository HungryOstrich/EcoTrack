using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcoTrack.Models;
using EcoTrack.DTOs;
using EcoTrack.Data; // Zakładam, że tu jest ApplicationDbContext

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
        // Wyświetlanie listy przy użyciu DTO
        public async Task<IActionResult> Index()
        {
            // Pobieramy dane wraz z relacjami (Eager Loading), aby DTO mogło pobrać nazwy
            var logs = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .Include(a => a.EmissionFactor)
                .AsNoTracking() // Optymalizacja dla operacji tylko do odczytu
                .ToListAsync();

            // Mapowanie na DTO
            var dtos = logs.Select(log => new ActivityLogDTO(log)).ToList();

            return View(dtos);
        }

        // GET: ActivityLogs/Create
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        // POST: ActivityLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Quantity,EmissionSourceId,EmissionFactorId")] ActivityLog activityLog)
        {
            // Musimy pobrać EmissionFactor z bazy, aby wykonać obliczenia biznesowe
            var factor = await _context.EmissionFactors.FindAsync(activityLog.EmissionFactorId);

            if (factor != null)
            {
                activityLog.EmissionFactor = factor;
                // KLUCZOWE: Wywołanie logiki biznesowej przed zapisem
                activityLog.CalculateEmission();
            }
            else
            {
                ModelState.AddModelError("EmissionFactorId", "Nie znaleziono wybranego czynnika emisji.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(activityLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Jeśli walidacja nie przejdzie (np. data z przyszłości), wracamy do formularza
            PopulateDropDowns(activityLog.EmissionSourceId, activityLog.EmissionFactorId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog == null) return NotFound();

            PopulateDropDowns(activityLog.EmissionSourceId, activityLog.EmissionFactorId);
            return View(activityLog);
        }

        // POST: ActivityLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Quantity,EmissionSourceId,EmissionFactorId")] ActivityLog activityLog)
        {
            if (id != activityLog.Id) return NotFound();

            // Ponowne pobranie czynnika, aby przeliczyć emisję w razie zmiany ilości lub czynnika
            var factor = await _context.EmissionFactors.FindAsync(activityLog.EmissionFactorId);

            if (factor != null)
            {
                activityLog.EmissionFactor = factor;
                // Aktualizacja obliczeń (snapshot)
                activityLog.CalculateEmission();
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
                    if (!ActivityLogExists(activityLog.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(activityLog.EmissionSourceId, activityLog.EmissionFactorId);
            return View(activityLog);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .Include(a => a.EmissionFactor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activityLog == null)
            {
                return NotFound();
            }
            
            return View(activityLog);
        }
        // GET: ActivityLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var activityLog = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .Include(a => a.EmissionFactor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activityLog == null) return NotFound();

            // Tutaj również możemy wyświetlić DTO, aby użytkownik widział czytelne dane przed usunięciem
            var dto = new ActivityLogDTO(activityLog);

            return View(dto);
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Metoda pomocnicza do ładowania list rozwijanych (SelectLists)
        private void PopulateDropDowns(int? selectedSource = null, int? selectedFactor = null)
        {
            ViewData["EmissionSourceId"] = new SelectList(_context.EmissionSources, "Id", "Name", selectedSource);
            // Wyświetlamy nazwę czynnika wraz z jednostką dla czytelności
            ViewData["EmissionFactorId"] = new SelectList(_context.EmissionFactors.Select(x => new
            {
                Id = x.Id,
                NameWithUnit = $"{x.Name} ({x.Unit})"
            }), "Id", "NameWithUnit", selectedFactor);
        }

        private bool ActivityLogExists(int id)
        {
            return _context.ActivityLogs.Any(e => e.Id == id);
        }
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoTrack.Data;
using EcoTrack.Models;
using EcoTrack.Models.ViewModels;

namespace EcoTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var totalEmissions = await _context.ActivityLogs.SumAsync(a => a.CalculatedCo2Emission);
            var logsCount = await _context.ActivityLogs.CountAsync();
            
            var activeSourcesCount = await _context.ActivityLogs
                .Select(a => a.EmissionSourceId)
                .Distinct()
                .CountAsync();

            var topSource = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .GroupBy(a => a.EmissionSource.Name)
                .OrderByDescending(g => g.Sum(x => x.CalculatedCo2Emission))
                .Select(g => g.Key)
                .FirstOrDefaultAsync() ?? "N/A";

            var recentLogs = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .OrderByDescending(a => a.Date)
                .Take(3)
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalEmissions = totalEmissions,
                TopSource = topSource,
                TreesEquivalent = (int)(totalEmissions / 25),
                RecentLogs = recentLogs,
                ReductionProgress = activeSourcesCount,
                PercentageChange = logsCount > 0 ? totalEmissions / logsCount : 0
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
namespace EcoTrack.Models.ViewModels
{
    public class DashboardViewModel
    {
        public double TotalEmissions { get; set; }
        public string TopSource { get; set; }
        public double ReductionProgress { get; set; }
        public int TreesEquivalent { get; set; }
        public List<ActivityLog> RecentLogs { get; set; }
        public double PercentageChange { get; set; }
    }
}
using System;

namespace SibintekTask.Core.Models
{
    public class RepairReport
    {
        public int RepairCount { get; set; }
        public int TotalSumRubles { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class VehicleReport
    {
        public int TotalSumRubles { get; set; }
        public int MiliagesPerPeriodInKm { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

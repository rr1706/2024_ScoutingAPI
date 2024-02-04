using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class TeamAverages_2023
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get; set; }
        public int teamNumber { get; set; }
        public decimal? autoBumpAvg { get; set; } = 0;
        public decimal? autoFlatAvg { get; set; } = 0;
        public decimal? autoMiddleAvg { get; set; } = 0;
        public decimal? autoMiddleEngage { get; set; } = 0;
        public decimal teleLowAvg { get; set; }
        public decimal teleMidAvg { get; set; }
        public decimal teleHighAvg { get; set; }
        public decimal totalTeleAvg { get; set; }
        public int autoBumpAttempts { get; set; }
        public int autoFlatAttempts { get; set; }
        public int autoMiddleAttempts { get; set; }
        public int numMatches { get; set; }
        public double? autoChargeStation { get; set; }    
        public double? endChargeStation { get; set; }

    }
}

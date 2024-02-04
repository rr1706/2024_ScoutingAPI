using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class TeamAverages_2023DTO
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get; set; }
        public int teamNumber { get; set; }
        public double? autoBumpAvg { get; set; } = 0; 
        public double? autoFlatAvg { get; set; } = 0;
        public double? autoMiddleAvg { get; set; } = 0;
        public double? autoMiddleEngage { get; set; } = 0;
        public double teleLowAvg { get; set; }
        public double teleMidAvg { get; set; }
        public double teleHighAvg { get; set; }
        public double totalTeleAvg { get; set; }
        public int autoBumpAttempts { get; set; }
        public int autoFlatAttempts { get; set; }
        public int autoMiddleAttempts { get; set; }
        public int numMatches { get; set; }
        public double autoChargeStation{ get; set; }
        public double endChargeStation { get; set; }
    }
}

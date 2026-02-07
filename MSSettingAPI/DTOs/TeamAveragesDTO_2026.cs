using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class TeamAverages_2026DTO
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string? eventCode { get; set; }
        public int teamNumber { get; set; }
        public int? totalAutoClimb { get; set; } = 0;
        public decimal? successfulAutoClimb { get; set; } = 0;
        public decimal? averageAutoFuelScored { get; set; } = 0;
        public decimal? averageAutoFuelFed { get; set; } = 0;
        public decimal? averageTeleFuelScored { get; set; } = 0;
        public decimal? averageTeleFuelFed { get; set; } = 0;
        public decimal? autoCOPR { get; set; } = 0;
        public decimal? teleCOPR { get; set; } = 0;
        public int? totalEndClimbTop { get; set; } = 0;
        public decimal? successfulEndClimbTop { get; set; } = 0;
        public int? totalEndClimbMiddle { get; set; } = 0;
        public decimal? successfulEndClimbMiddle { get; set; } = 0;
        public int? totalEndClimbBottom { get; set; } = 0;
        public decimal? successfulEndClimbBottom { get; set; } = 0;
        public decimal? averageEndClimbPoints { get; set; } = 0;
        public decimal? averageShotAccuracy { set; get; } = 0;
        public decimal? averageShotRate { set; get; } = 0;
        public int? numMatches { get; set; } = 0;
        public bool? doNotPick { get; set; } = false;
        public decimal? averageTotalPoints { get; set; } = 0;
        public decimal? averageAutoPoints { get; set; } = 0;
        public decimal? averageTeleClimbPoints { get; set; } = 0;
        public decimal? averageTeleTotalPoints { get; set; } = 0;
    }
}
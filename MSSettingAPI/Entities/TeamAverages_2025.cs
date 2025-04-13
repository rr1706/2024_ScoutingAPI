using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class TeamAverages_2025
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string? eventCode { get; set; }
        public int teamNumber { get; set; }
        public decimal? averageAutoCoral { get; set; } = 0;
        public decimal? averageTeleCoral { get; set; } = 0;
        public decimal? averageReefRemoval { get; set; } = 0;
        public decimal? averageBargeAll { get; set; } = 0;
        public decimal? averageProcessorAll { get; set; } = 0;
        public decimal? successfulDeepClimb { get; set; } = 0;
        public int? totalDeepClimb { get; set; } = 0;
        public decimal? successfulShallowClimb { get; set; } = 0;
        public int? totalShallowClimb { get; set; } = 0;
        public decimal? percentMoblilitize { get; set; } = 0;
        public decimal? totalPoints { get; set; } = 0;
        public int? numMatches { get; set; } = 0;
        public int? doNotPick { get; set; } = 0;
        public decimal? sideCoralAuto { get; set; } = 0;
        public decimal? middleCoralAuto { get; set; } = 0;
        public decimal? middleNetAuto { get; set; } = 0;
        public int? sideAutoCount { get; set; } = 0;
        public int? middleAutoCount { get; set; } = 0;
        public int? offensiveCount { get; set; } = 0;
        public decimal? defendedScored { get; set; } = 0;
        public decimal? unDefendedScored { get; set; } = 0;
        public int? defendedCount { get; set; } = 0;
        public int? unDefendedCount { get; set; } = 0;
        public decimal? totalTeleScore { get; set; } = 0;
        public decimal? totalTeleAdjusted { get; set; } = 0;
    }
}

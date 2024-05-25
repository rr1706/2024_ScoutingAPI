using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class TeamAverages_2024DTO
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get; set; }
        public int teamNumber { get; set; }
        public decimal? autoAmpAvg { get; set; } = 0;
        public decimal? autoSpeakerAvg { get; set; } = 0;
        public decimal? autoTotalAvg { get; set; } = 0;
        public decimal? teleAmpAvg { get; set; } = 0;
        public decimal? teleSpeakerAvg { get; set; } = 0;
        public decimal? teleTrapAvg { get; set; } = 0;
        public decimal? teleTotalAvg { get; set; } = 0;
        public decimal? totalAvg { get; set; } = 0;
        public decimal climbPercent { get; set; } = 0;
        public decimal climbSuccessRate { get; set; } = 0;
        public int climbAttempts { get; set; } = 0;
        public int numMatches { get; set; } = 0;
        public decimal totalPoints { get; set; } = 0;
        public decimal closeAutoAvg { get; set; } = 0;
        public int closeAutoNum { get; set; } = 0;
        public decimal? feedAvg { get; set; } = 0;
        public decimal centerAutoAvg { get; set; } = 0;
        public int centerAutoNum { get; set; } = 0;
        public int? maxFeeds { get; set; } = 0;
    }
}

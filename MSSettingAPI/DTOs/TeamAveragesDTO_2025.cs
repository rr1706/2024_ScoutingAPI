using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class TeamAverages_2025DTO
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string? eventCode { get; set; }
        public int teamNumber { get; set; }
        public decimal? averageAutoCoral { get; set; } = 0;
        public decimal? averageTeleCoral { get; set; } = 0;
        public decimal? averageBargeAll { get; set; } = 0;
        public decimal? averageProcessorAll { get; set; } = 0;
        public int? successfulDeepClimb { get; set; } = 0;
        public int? totalDeepClimb { get; set; } = 0;
        public int? successfulShallowClimb { get; set; } = 0;
        public int? totalShallowClimb { get; set; } = 0;
        public decimal? percentMoblilitize { get; set; } = 0;
        public decimal? totalPoints { get; set; } = 0;
        public int? numMatches { get; set; } = 0;
        public int? doNotPick { set; get; } = 0;
    }
}
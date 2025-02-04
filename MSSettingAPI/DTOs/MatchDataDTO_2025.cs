using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class MatchDataDTO_2025
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string? eventCode { get; set; }
        public int teamNumber { get; set; }
        public int matchNumber { get; set; }
        public int coralL1 { get; set; }
        [MaxLength(30)]
        public int coralL2 { get; set; }
        [MaxLength(12)]
        public int coralL3 { get; set; }
        [MaxLength(12)]
        public int coralL4 { get; set; }
        [MaxLength(12)]
        public int autoCoralL1 { get; set; }
        [MaxLength(30)]
        public int autoCoralL2 { get; set; }
        [MaxLength(12)]
        public int autoCoralL3 { get; set; }
        [MaxLength(12)]
        public int autoCoralL4 { set; get; }
        [MaxLength(12)]
        public int processor { set; get; }
        [MaxLength(35)]
        public int autoProcessor { set; get; }
        [MaxLength(35)]
        public string? endClimb { set; get; }
        public int groundAlgae { set; get; }
        public int reefAlgae { set; get; }
        public int autoGroundAlgae { set; get; }
        public int autoReefAlgae { set; get; }
        public int barge { set; get; }
        [MaxLength(18)]
        public int autoBarge { get; set; }
        [MaxLength(18)]
        public int defence { get; set; }
        [MaxLength(1)]
        public int defended { get; set; }
        [MaxLength(1)]
        public int mobilitize { get; set; }
        [MaxLength(1)]
        public int gambleAmount { get; set; }
        [MaxLength(5)]
        public string? notes { set; get; }
        public string? gambleColor { set; get; }

        public string? scoutName { get; set; }
        public int ignore { get; set; }

        public int doNotPick { get; set; }
    }
}

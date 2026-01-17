using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class MatchDataDTO_2026
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string? eventCode { get; set; }
        public int teamNumber { get; set; }
        public int matchNumber { get; set; }
        [Range(0, 30, ErrorMessage = " Coral not within range")]
        public int coralL1 { get; set; }
        [Range(0, 12, ErrorMessage = " Coral not within range")]
        public int coralL2 { get; set; }
        [Range(0, 12, ErrorMessage = " Coral not within range")]
        public int coralL3 { get; set; }
        [Range(0, 12, ErrorMessage = " Coral not within range")]
        public int coralL4 { get; set; }
        [Range(0, 30, ErrorMessage = "Auto Coral not within range")]
        public int autoCoralL1 { get; set; }
        [Range(0, 12, ErrorMessage = "Auto Coral not within range")]
        public int autoCoralL2 { get; set; }
        [Range(0, 12, ErrorMessage = "Auto Coral not within range")]
        public int autoCoralL3 { get; set; }
        [Range(0, 12, ErrorMessage = "Auto Coral not within range")]
        public int autoCoralL4 { set; get; }
        [Range(0, 35, ErrorMessage = "Processor not within range")]
        public int processor { set; get; }
        [Range(0, 35, ErrorMessage = "Auto Processor not within range")]
        public int autoProcessor { set; get; }
        public string? endClimb { set; get; }
        public int groundAlgae { set; get; }
        public int reefAlgae { set; get; }
        public int autoGroundAlgae { set; get; }
        public int autoReefAlgae { set; get; }
        [Range(0, 18, ErrorMessage = "Barge L1 not within range")]
        public int barge { set; get; }
        [Range(0, 18, ErrorMessage = "Auto Barge L1 not within range")]
        public int autoBarge { get; set; }
        [Range(0, 1, ErrorMessage = "Defence L1 not within range")]
        public int defence { get; set; }
        [Range(0, 1, ErrorMessage = "Defended L1 not within range")]
        public int defended { get; set; }
        [Range(0, 1, ErrorMessage = "Mobilitize L1 not within range")]
        public int mobilitize { get; set; }
        [Range(0, 5, ErrorMessage = "Gamble Amount L1 not within range")]
        public int gambleAmount { get; set; }
        public string? notes { set; get; }
        public string? gambleColor { set; get; }
        public string? scoutName { get; set; }
        public int ignore { get; set; }
        public int doNotPick { get; set; }
        public int? edited { get; set; }
        public string? autoPosition { get; set; }
        public int? validatedClimb { get; set; }
    }
}

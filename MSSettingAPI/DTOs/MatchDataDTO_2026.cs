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
        public int teleFuelScored { get; set; }
        public int autoFuelScored { get; set; }
        public int teleFuelFed { get; set; }
        public int autoFuelFed { get; set; }
        public string? endClimb { set; get; }
        public bool fuelSourceMidfield { set; get; }
        public bool fuelSourceDepot { set; get; }
        public bool fuelSourceHP { set; get; }
        public bool fuelSourcePreLoadOnly { set; get; }
        public string? autoClimb { set; get; }
        [Range(0, 5, ErrorMessage = "Fuel Shot Accuracy not within range")]
        public int shotAccuracy { set; get; }
        [Range(0, 5, ErrorMessage = "Fuel Shot Rate not within range")]
        public int shotRate { set; get; }
        public string? defense { get; set; }
        [Range(0, 5, ErrorMessage = "Gamble Amount not within range")]
        public int gambleAmount { get; set; }
        public string? notes { set; get; }
        public string? gambleColor { set; get; }
        public string? scoutName { get; set; }
        public bool? ignore { get; set; } = false;
        public bool? edited { get; set; }
        public int endClimbPoints { get; set; }
    }
}

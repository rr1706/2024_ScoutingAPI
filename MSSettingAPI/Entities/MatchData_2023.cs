using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class MatchData_2023
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get;set; }
        public int teamNumber { get; set; }
        public int matchNumber { get; set; }
        public string startingPosition { get; set; }
        public int autoLow { get; set; }
        public int autoMid { get; set; }
        public int autoHigh { get; set; }
        public string? autoChargeStation { get; set; }
        public int teleLow { get; set; }
        public int teleMid { get; set; }
        public int teleHigh { get; set; }
        public string? endChargeStation { get; set; }  
        public string? comment { get; set; }    
    }
}

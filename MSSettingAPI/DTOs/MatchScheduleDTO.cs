using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class MatchScheduleDTO
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get; set; }
        public int matchNumber { get; set; }
        public int red1 { get; set; }
        public int red2 { get; set; }
        public int red3 { get; set; }
        public int blue1 { get; set; }
        public int blue2 { get; set; }
        public int blue3 { get; set; }
    }
}

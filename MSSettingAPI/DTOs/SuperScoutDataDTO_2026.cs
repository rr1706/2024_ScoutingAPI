using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class SuperScoutDataDTO_2026
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get; set; }
        public int teamNumber { get; set; }
        public int matchNumber { get; set; }
        public string scoutName { get; set; }
        public string comments { get; set; }
        public string type { get; set; }
        public int? shotRate { get; set; }
        public int? shotAccuracy { get; set; }

    }
}

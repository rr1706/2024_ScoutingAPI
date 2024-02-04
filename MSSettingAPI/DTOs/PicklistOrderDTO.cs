using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class PicklistOrderDTO
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string eventCode { get; set; }
        [Required]
        public int teamNumber { get; set; }
        [Required]
        public int order { get; set; }
    }
}

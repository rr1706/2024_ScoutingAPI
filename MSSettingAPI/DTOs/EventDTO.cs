using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class EventDTO
    {
        [Key]
        [Required]
        public string eventCode { get; set; }
        [Required]
        public string eventName { get; set; }
    }
}

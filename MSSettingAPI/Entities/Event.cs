using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class Event
    {
        [Key]
        [Required]
        public string eventCode { get;set; }
        [Required]
        public string eventName { get; set; }
    }
}

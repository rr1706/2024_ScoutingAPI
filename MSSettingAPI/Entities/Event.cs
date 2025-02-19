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

        public int year { get; set; }

        public string? tbaCode { get; set; }


    }
}

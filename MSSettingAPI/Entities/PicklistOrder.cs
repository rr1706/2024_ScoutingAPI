using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class PicklistOrder
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string eventCode { get;set; }
        [Required]
        public int teamNumber { get; set; }
        [Required]
        public int order { get; set; }
        [Required]
        public string email { get; set; }
    }
}

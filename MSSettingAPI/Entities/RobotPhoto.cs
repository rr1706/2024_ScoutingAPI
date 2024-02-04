using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class RobotPhoto
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get; set; }
        [Required]
        public int teamNumber { get; set; }
        [Required]
        public string picture { get; set; }
    }
}

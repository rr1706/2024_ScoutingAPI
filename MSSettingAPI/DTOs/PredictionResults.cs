using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class PredictionsDTO
    {
        [Required]
        public string scoutName { get; set; }
        [Required]

        public int score { get; set; }

        public int numberMatches { get; set; }

    }
}

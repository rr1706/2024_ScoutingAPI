using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class ValidatedMatchDTO
    {
        public int matchNumber { get; set; }
        public List<int> teamNumbers { get; set; }
        public string field { get; set; }
        public int currentValue { get; set; }
        public int correctValue { get; set; }   
        public string matchVideo { get; set; }
    }
}

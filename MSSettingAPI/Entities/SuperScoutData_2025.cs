using System.ComponentModel.DataAnnotations;

public class SuperScoutData_2025
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

}


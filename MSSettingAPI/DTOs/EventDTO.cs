﻿using System.ComponentModel.DataAnnotations;

namespace RRScout.DTOs
{
    public class EventDTO
    {
        [Key]
        [Required]
        public string eventCode { get; set; }
        [Required]
        public string eventName { get; set; }

        public int year { get; set; }

        public string? tbaCode { get; set; }
    }
}

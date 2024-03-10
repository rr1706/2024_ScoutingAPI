﻿using System.ComponentModel.DataAnnotations;

namespace RRScout.Entities
{
    public class MatchData_2024
    {
        [Key]
        [Required]
        public int id { get; set; }
        public string eventCode { get;set; }
        public int teamNumber { get; set; }
        public int matchNumber { get; set; }
        public int autoAmp { get; set; }
        public int autoSpeaker { get; set; }
        public int teleAmp { get; set; }
        public int teleSpeaker { get; set; }
        public int teleTrap { get; set; }
        [MaxLength(20)]
        public string? climb { get; set; }
        [MaxLength(3)]
        public string? playedDefense { get; set; }
        [MaxLength(500)]
        public string? comment { get; set; }
        [MaxLength(5)]
        public string? autoPreload { set; get; }
        [MaxLength(5)]
        public string? autoClose1 { set; get; }
        [MaxLength(5)]
        public string? autoClose2 { set; get; }
        [MaxLength(5)]
        public string? autoClose3 { set; get; }
        [MaxLength(5)]
        public string? autoCenter1 { set; get; }
        [MaxLength(5)]
        public string? autoCenter2 { set; get; }
        [MaxLength(5)]
        public string? autoCenter3 { set; get; }
        [MaxLength(5)]
        public string? autoCenter4 { set; get; }
        [MaxLength(5)]
        public string? autoCenter5 { set; get; }
        public int? teleFeeds { set; get; }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRScout.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace RRScout
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MatchData_2023> MatchData_2023 { get; set; }
        public DbSet<TeamAverages_2023> TeamAverages_2023 { get; set; }
        public DbSet<MatchData_2024> MatchData_2024 { get; set; }
        public DbSet<TeamAverages_2024> TeamAverages_2024 { get; set; }
        public DbSet<PicklistOrder> PicklistOrder { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<MatchSchedule> MatchSchedule { get; set; }

        public DbSet<RobotPhoto> RobotPhotos { get; set; }

        public DbSet<TeamAverages_2025> TeamAverages_2025 { get; set; }
        public DbSet<MatchData_2025> MatchData_2025 { get; set; }

        public DbSet<SuperScoutData_2025> SuperScoutData_2025 { get; set; }
        public DbSet<TeamNames> TeamNames { get; set; }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRScout.Entities;
using System;
using System.Globalization;

namespace RRScout.Controllers
{
    [Route("api/teamaverages2024")]
    [ApiController]
    public class TeamAverages2024 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public TeamAverages2024(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpGet("calculateAverages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Calculate(string eventID)
        {
            try
            {
                var matchData = await Context.MatchData_2024.Where(x => x.eventCode == eventID).OrderBy(x => x.teamNumber).ToListAsync();

                var newAverages = new List<TeamAverages_2024>();

                int currentTeam = 0;
                TeamAverages_2024 newAverage = new TeamAverages_2024();

                foreach (var match in matchData)
                {
                    if (match.teamNumber != currentTeam)
                    {
                        if (currentTeam != 0)
                        {
                            CalculateAverages(newAverage);
                            newAverages.Add(newAverage);
                            newAverage = new TeamAverages_2024();
                        }
                        newAverage.teamNumber = match.teamNumber;
                        newAverage.eventCode = match.eventCode;
                        AddMatch(newAverage, match);

                    }
                    else
                    {
                        AddMatch(newAverage, match);
                    }
                    currentTeam = match.teamNumber;
                }
                CalculateAverages(newAverage);
                newAverages.Add(newAverage);

                var oldAverages = await Context.TeamAverages_2024.Where(x => x.eventCode == eventID).ToListAsync();
                Context.TeamAverages_2024.RemoveRange(oldAverages);

                Context.TeamAverages_2024.AddRange(newAverages);

                Context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("getteamaverages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<TeamAverages_2024>>> GetTeamAverages(string eventID)
        {
            var teamAverages = await Context.TeamAverages_2024.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(teamAverages);
        }


        private static void AddMatch(TeamAverages_2024 newAverage, MatchData_2024 match)
        {
            newAverage.numMatches += 1;
            newAverage.autoAmpAvg += match.autoAmp;
            newAverage.autoSpeakerAvg += match.autoSpeaker;
            newAverage.teleAmpAvg += match.teleAmp;
            newAverage.teleSpeakerAvg += match.teleSpeaker;
            newAverage.teleTrapAvg += match.teleTrap;
            if (match.climb == "Yes")
            {
                newAverage.climbPercent += 1;
            }
            if (match.climb == "Yes" || match.climb == "Fail")
            {
                newAverage.climbAttempts += 1;
            }
        }

        private static void CalculateAverages(TeamAverages_2024 newAverage)
        {
            newAverage.autoAmpAvg = newAverage.autoAmpAvg / newAverage.numMatches;
            newAverage.autoSpeakerAvg = newAverage.autoSpeakerAvg / newAverage.numMatches;
            newAverage.teleSpeakerAvg = newAverage.teleSpeakerAvg / newAverage.numMatches;
            newAverage.teleAmpAvg = newAverage.teleAmpAvg / newAverage.numMatches;
            newAverage.teleTrapAvg = newAverage.teleTrapAvg / newAverage.numMatches;

            newAverage.autoTotalAvg = newAverage.autoSpeakerAvg + newAverage.autoAmpAvg;
            newAverage.teleTotalAvg = newAverage.teleSpeakerAvg + newAverage.teleAmpAvg;
            newAverage.totalAvg = newAverage.autoTotalAvg + newAverage.teleTotalAvg;

            newAverage.climbSuccessRate = newAverage.climbPercent;
            newAverage.climbPercent = (newAverage.climbPercent / newAverage.numMatches)*100;

            if (newAverage.climbAttempts > 0)
            {
                newAverage.climbSuccessRate = (newAverage.climbSuccessRate / newAverage.climbAttempts) * 100;
            }
            else
            {
                newAverage.climbSuccessRate = 0;
            }
            

            newAverage.totalPoints =
                (decimal)((newAverage.autoAmpAvg * 2) +
                (newAverage.autoSpeakerAvg * 5) +
                (newAverage.teleSpeakerAvg * 5) +
                (newAverage.teleAmpAvg * 1) +
                (newAverage.teleTrapAvg * 5) +
                (newAverage.climbPercent / 100 * 3));

        }
    }


}

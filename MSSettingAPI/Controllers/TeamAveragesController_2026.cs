using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRScout.Entities;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RRScout.Controllers
{
    [Route("api/teamaverages2026")]
    [ApiController]
    public class TeamAverages2026 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public TeamAverages2026(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpGet("calculateAverages")]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Calculate(string eventID)
        {
            try
            {
                var matchData = await Context.MatchData_2026.Where(x => x.eventCode == eventID).OrderBy(x => x.teamNumber).ToListAsync();

                var newAverages = new List<TeamAverages_2026>();
                int currentTeam = 0;
                TeamAverages_2026 newAverage = new TeamAverages_2026();

                foreach (var match in matchData)
                {
                    if (match.teamNumber != currentTeam)
                    {
                        if (currentTeam != 0)
                        {
                            CalculateAverages(newAverage);
                            newAverages.Add(newAverage);
                            newAverage = new TeamAverages_2026();
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

                var oldAverages = await Context.TeamAverages_2026.Where(x => x.eventCode == eventID).ToListAsync();
                Context.TeamAverages_2026.RemoveRange(oldAverages);

                Context.TeamAverages_2026.AddRange(newAverages);

                Context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getteamaverages")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<TeamAverages_2026>>> GetTeamAverages(string eventID)
        {
            try
            {
                var teamAverages = await Context.TeamAverages_2026.Where(x => x.eventCode == eventID).ToListAsync();
                return Ok(teamAverages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private static void AddMatch(TeamAverages_2026 newAverage, MatchData_2026 match)
        {
            if (!match.ignore)
            {
                newAverage.numMatches += 1;

                newAverage.averageAutoFuelScored += match.autoFuelScored;
                newAverage.averageAutoFuelFed += match.autoFuelFed;
                newAverage.averageTeleFuelScored += match.teleFuelScored;
                newAverage.averageTeleFuelFed += match.teleFuelFed;
                newAverage.averageShotAccuracy += match.shotAccuracy;
                newAverage.averageShotRate += match.shotRate;

                newAverage.averageTeleTotalPoints += match.teleFuelScored;
                newAverage.averageTotalPoints += match.teleFuelScored;

                if (match.endClimb == "top") 
                { 
                    newAverage.averageTeleTotalPoints += 30;
                    newAverage.successfulEndClimbTop++;
                    newAverage.totalEndClimbTop++;
                    newAverage.averageEndClimbPoints += 30;
                    newAverage.averageTotalPoints += 30;
                }
                if (match.endClimb == "middle")
                {
                    newAverage.averageTeleTotalPoints += 20;
                    newAverage.successfulEndClimbMiddle++;
                    newAverage.totalEndClimbMiddle++;
                    newAverage.averageEndClimbPoints += 20;
                    newAverage.averageTotalPoints += 20;
                }
                if (match.endClimb == "bottom")
                {
                    newAverage.averageTeleTotalPoints += 10;
                    newAverage.successfulEndClimbBottom++;
                    newAverage.totalEndClimbBottom++;
                    newAverage.averageEndClimbPoints += 10;
                    newAverage.averageTotalPoints += 10;
                }
                if (match.endClimb == "fail")
                {
                    newAverage.totalEndClimbTop++;
                    newAverage.totalEndClimbMiddle++;
                    newAverage.totalEndClimbBottom++;
                }

                newAverage.averageAutoTotalPoints += match.autoFuelScored;
                newAverage.averageTotalPoints += match.autoFuelScored;

                if (match.autoClimb == "succeed")
                {
                    newAverage.averageAutoTotalPoints += 15;
                    newAverage.successfulAutoClimb++;
                    newAverage.totalAutoClimb++;
                    newAverage.averageTotalPoints += 15;
                }
                if (match.autoClimb == "fail")
                {
                    newAverage.totalAutoClimb++;
                }
            }
        }


        private static void CalculateAverages(TeamAverages_2026 newAverage)
        {
            if (newAverage.numMatches < 1)
            {
                return;
            }
            //Here we store the number of times the team has successfully done that particular climb
            decimal tempA = newAverage.successfulAutoClimb ?? 0;

            decimal tempT = newAverage.successfulEndClimbTop ?? 0;
            decimal tempM = newAverage.successfulEndClimbMiddle ?? 0;
            decimal tempB = newAverage.successfulEndClimbBottom ?? 0;
            
            newAverage.averageAutoFuelScored = newAverage.averageAutoFuelScored / newAverage.numMatches;
            newAverage.averageAutoFuelFed = newAverage.averageAutoFuelFed / newAverage.numMatches;

            newAverage.averageTeleFuelScored = newAverage.averageTeleFuelScored / newAverage.numMatches;
            newAverage.averageTeleFuelFed = newAverage.averageTeleFuelFed / newAverage.numMatches;

            newAverage.averageEndClimbPoints = newAverage.averageEndClimbPoints / newAverage.numMatches;

            newAverage.averageShotAccuracy = newAverage.averageShotAccuracy / newAverage.numMatches;
            newAverage.averageShotRate = newAverage.averageShotRate / newAverage.numMatches;

            //Here the successful___climb has been repurposed to store the percentage of the time that the team was successful in doing that climb
            //And total___climb is storing the number of times that type of climb has been attempted, which is not a metric we display but we need for calculations
            if (newAverage.totalAutoClimb != 0)
            {
                newAverage.successfulAutoClimb = (newAverage.successfulAutoClimb / newAverage.totalAutoClimb) * 100;
            }

            if (newAverage.totalEndClimbTop != 0)
            {
                newAverage.successfulEndClimbTop = (newAverage.successfulEndClimbTop / newAverage.totalEndClimbTop) * 100;
            }
            if (newAverage.totalEndClimbMiddle != 0)
            {
                newAverage.successfulEndClimbMiddle = (newAverage.successfulEndClimbMiddle / newAverage.totalEndClimbMiddle) * 100;
            }
            if (newAverage.totalEndClimbBottom != 0)
            {
                newAverage.successfulEndClimbBottom = (newAverage.successfulEndClimbBottom / newAverage.totalEndClimbBottom) * 100;
            }

            //Then we set total___climb equal to that actual final metric we DO display, the number of times the team has successfully done that particular climb
            newAverage.totalAutoClimb = Decimal.ToInt32(tempA);
            newAverage.totalEndClimbTop = Decimal.ToInt32(tempT);
            newAverage.totalEndClimbMiddle = Decimal.ToInt32(tempM);
            newAverage.totalEndClimbBottom = Decimal.ToInt32(tempB);

            newAverage.averageAutoTotalPoints = newAverage.averageAutoTotalPoints / newAverage.numMatches;
            newAverage.averageTeleTotalPoints = newAverage.averageTeleTotalPoints / newAverage.numMatches;
            newAverage.averageTotalPoints = newAverage.averageTotalPoints / newAverage.numMatches;
        }
    }
}

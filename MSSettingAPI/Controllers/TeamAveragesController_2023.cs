using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRScout.Entities;
using System;
using System.Globalization;

namespace RRScout.Controllers
{
    [Route("api/teamaverages")]
    [ApiController]
    public class TeamAverages : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public TeamAverages(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpGet("calculateAverages")]
        public async Task<ActionResult> Calculate(string eventID)
        {
            var matchData = await Context.MatchData_2023.Where(x => x.eventCode == eventID).OrderBy(x => x.teamNumber).ToListAsync();

            var newAverages = new List<TeamAverages_2023>();

            int currentTeam = 0;
            TeamAverages_2023 newAverage = new TeamAverages_2023();

            foreach (var match in matchData) { 
                if (match.teamNumber != currentTeam)
                {
                    if (currentTeam != 0)
                    {
                        CalculateAverages(newAverage);
                        newAverages.Add(newAverage);
                        newAverage = new TeamAverages_2023();
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

            var oldAverages = await Context.TeamAverages_2023.Where(x => x.eventCode == eventID).ToListAsync();
            Context.TeamAverages_2023.RemoveRange(oldAverages);

            Context.TeamAverages_2023.AddRange(newAverages);
            Context.SaveChanges();

            return Ok();
        }

        [HttpGet("getteamaverages")]
        public async Task<ActionResult<List<int>>> GetTeamAverages(string eventID)
        {
            var teamAverages = await Context.TeamAverages_2023.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(teamAverages);
        }


        private static void AddMatch(TeamAverages_2023 newAverage, MatchData_2023 match)
        {
            newAverage.numMatches += 1;
            newAverage.teleLowAvg += match.teleLow;
            newAverage.teleMidAvg += match.teleMid;
            newAverage.teleHighAvg += match.teleHigh;
            if (match.startingPosition == "Flat")
            {
                newAverage.autoFlatAttempts += 1;
                newAverage.autoFlatAvg += (match.autoLow + match.autoMid + match.autoHigh);
            }
            else if (match.startingPosition == "Bump")
            {
                newAverage.autoBumpAttempts += 1;
                newAverage.autoBumpAvg += (match.autoLow + match.autoMid + match.autoHigh);
            }
            else if (match.startingPosition == "Middle")
            {
                newAverage.autoMiddleAttempts += 1;
                newAverage.autoMiddleAvg += (match.autoLow + match.autoMid + match.autoHigh);
                if (match.autoChargeStation == "ENGAGED" || match.autoChargeStation == "DOCKED")
                {
                    newAverage.autoMiddleEngage += 1;
                } 
            }
            if (match.autoChargeStation == "Engaged" || match.autoChargeStation == "Docked")
            {
                newAverage.autoChargeStation += 1;
            }
            if (match.endChargeStation == "Engaged" || match.endChargeStation == "Docked")
            {
                newAverage.endChargeStation += 1;
            }
        }

        private static void CalculateAverages(TeamAverages_2023 newAverage)
        {
            if (newAverage.autoBumpAttempts > 0){
                newAverage.autoBumpAvg = newAverage.autoBumpAvg / newAverage.autoBumpAttempts;
            }else{
                newAverage.autoBumpAvg = null;
            }

            if (newAverage.autoFlatAttempts > 0)
            {
                newAverage.autoFlatAvg = newAverage.autoFlatAvg / newAverage.autoFlatAttempts;
            }else {
                newAverage.autoFlatAvg = null;
            }

            if (newAverage.autoMiddleAttempts > 0)
            {
                newAverage.autoMiddleAvg = newAverage.autoMiddleAvg / newAverage.autoMiddleAttempts;
                newAverage.autoMiddleEngage = (newAverage.autoMiddleEngage / newAverage.autoMiddleAttempts)*100;
            } else{
                newAverage.autoMiddleAvg = null;
                newAverage.autoMiddleEngage = null;
            }
            newAverage.teleLowAvg = newAverage.teleLowAvg / newAverage.numMatches;
            newAverage.teleMidAvg = newAverage.teleMidAvg / newAverage.numMatches;
            newAverage.teleHighAvg = newAverage.teleHighAvg / newAverage.numMatches;
            newAverage.totalTeleAvg = newAverage.teleLowAvg + newAverage.teleMidAvg + newAverage.teleHighAvg;

            newAverage.autoChargeStation = (newAverage.autoChargeStation / newAverage.numMatches)*100;
            newAverage.endChargeStation = (newAverage.endChargeStation / newAverage.numMatches)*100;

        }
    }


}

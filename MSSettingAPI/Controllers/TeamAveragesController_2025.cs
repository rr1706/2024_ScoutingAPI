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
    [Route("api/teamaverages2025")]
    [ApiController]
    public class TeamAverages2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public TeamAverages2025(ApplicationDbContext context, IMapper mapper)
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
                var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID).OrderBy(x => x.teamNumber).ToListAsync();

                var newAverages = new List<TeamAverages_2025>();
                int currentTeam = 0;
                TeamAverages_2025 newAverage = new TeamAverages_2025();

                foreach (var match in matchData)
                {
                    if (match.teamNumber != currentTeam)
                    {
                        if (currentTeam != 0)
                        {
                            CalculateAverages(newAverage);
                            newAverages.Add(newAverage);
                            newAverage = new TeamAverages_2025();
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

                var oldAverages = await Context.TeamAverages_2025.Where(x => x.eventCode == eventID).ToListAsync();
                Context.TeamAverages_2025.RemoveRange(oldAverages);

                Context.TeamAverages_2025.AddRange(newAverages);

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
        public async Task<ActionResult<List<TeamAverages_2025>>> GetTeamAverages(string eventID)
        {
            try
            {
                var teamAverages = await Context.TeamAverages_2025.Where(x => x.eventCode == eventID).ToListAsync();
                return Ok(teamAverages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private static void AddMatch(TeamAverages_2025 newAverage, MatchData_2025 match)
        {
            if (match.ignore == 0)
            {
                newAverage.numMatches += 1;
                if (match.autoPosition == "Middle")
                {
                    newAverage.middleCoralAuto += (match.autoCoralL4 + match.autoCoralL3 + match.autoCoralL2 + match.autoCoralL1);
                    newAverage.middleAutoCount += 1;
                    newAverage.middleNetAuto += (match.autoBarge);
                }else if (match.autoPosition == "Side")
                {
                    newAverage.sideCoralAuto += (match.autoCoralL4 + match.autoCoralL3 + match.autoCoralL2 + match.autoCoralL1);
                    newAverage.sideAutoCount += 1;
                }
                newAverage.averageAutoCoral += (match.autoCoralL4 + match.autoCoralL3 + match.autoCoralL2 + match.autoCoralL1);

                

                if (match.defence == 0)
                {
                    newAverage.averageProcessorAll += (match.autoProcessor + match.processor);
                    newAverage.averageBargeAll += (match.barge + match.autoBarge);
                    newAverage.averageTeleCoral += (match.coralL4 + match.coralL3 + match.coralL2 + match.coralL1);
                    newAverage.averageReefRemoval += (match.autoReefAlgae + match.reefAlgae);
                    newAverage.offensiveCount += 1;
                    newAverage.totalTeleScore += (match.coralL4 + match.coralL3 + match.coralL2 + match.coralL1 + match.processor + match.barge);
                    newAverage.totalTeleAdjusted += (match.coralL4 + match.coralL3 + match.coralL2 + match.coralL1 + (match.processor * 1.75m) + (match.barge * 1.75m));

                    if (match.defended == 0)
                    {
                        newAverage.unDefendedScored += match.autoProcessor + match.processor + match.barge + match.autoBarge + match.coralL4 + match.coralL3 + match.coralL2 + match.coralL1;
                        newAverage.unDefendedCount += 1;
                    }
                    else
                    {
                        newAverage.defendedScored += match.autoProcessor + match.processor + match.barge + match.autoBarge + match.coralL4 + match.coralL3 + match.coralL2 + match.coralL1;
                        newAverage.defendedCount += 1;
                    }
                }

                newAverage.percentMoblilitize += match.mobilitize;
                if (match.endClimb == "Deep")
                {
                    newAverage.successfulDeepClimb += 1;
                    newAverage.totalDeepClimb += 1;
                }
                if (match.endClimb == "Shallow")
                {
                    newAverage.successfulShallowClimb += 1;
                    newAverage.totalShallowClimb += 1;
                }
                if (match.endClimb == "Fail")
                {
                    //This is the Speghetti w/ paint over the hole
                    newAverage.totalDeepClimb += 1;
                    newAverage.totalShallowClimb += 1;
                }
                newAverage.totalPoints += ((match.autoProcessor + match.processor) * 2) + ((match.barge + match.autoBarge) * 4) + (match.autoCoralL4 * 7) + (match.autoCoralL3 * 6) + (match.autoCoralL2 * 4) + (match.autoCoralL1 * 3) + (match.coralL4 * 5) + (match.coralL3 * 4) + (match.coralL2 * 3) + (match.coralL1 * 2) + (match.mobilitize * 3);
                if (match.endClimb == "Deep")
                {
                    newAverage.totalPoints += 12;
                }
                if (match.endClimb == "Shallow")
                {
                    newAverage.totalPoints += 6;
                }
            }
        }


        private static void CalculateAverages(TeamAverages_2025 newAverage)
        {
            if (newAverage.numMatches < 1)
            {
                return;
            }
            //More Speget
            decimal tempD = newAverage.successfulDeepClimb ?? 0;
            decimal tempS = newAverage.successfulShallowClimb ?? 0;
            //End of Speget
            if (newAverage.sideAutoCount != 0)
            {
                newAverage.sideCoralAuto = newAverage.sideCoralAuto / newAverage.sideAutoCount;
            }
            if (newAverage.middleAutoCount != 0)
            {
                newAverage.middleCoralAuto = newAverage.middleCoralAuto / newAverage.middleAutoCount;
                newAverage.middleNetAuto = newAverage.middleNetAuto / newAverage.middleAutoCount;
            }

            
            newAverage.averageAutoCoral = newAverage.averageAutoCoral / newAverage.numMatches;

            if (newAverage.offensiveCount != 0)
            {
                newAverage.averageTeleCoral = newAverage.averageTeleCoral / newAverage.offensiveCount;
                newAverage.averageProcessorAll = newAverage.averageProcessorAll / newAverage.offensiveCount;
                newAverage.averageBargeAll = newAverage.averageBargeAll / newAverage.offensiveCount;
                newAverage.averageReefRemoval = newAverage.averageReefRemoval / newAverage.offensiveCount;
            }

            if(newAverage.totalTeleScore != 0)
            {
                newAverage.totalTeleScore = newAverage.totalTeleScore / newAverage.offensiveCount;
                newAverage.totalTeleAdjusted = newAverage.totalTeleAdjusted / newAverage.offensiveCount;
            }

            if (newAverage.defendedCount != 0)
            {
                newAverage.defendedScored = newAverage.defendedScored / newAverage.defendedCount;
            }
            if (newAverage.unDefendedCount != 0)
            {
                newAverage.unDefendedScored = newAverage.unDefendedScored / newAverage.unDefendedCount;
            }
            

            if (newAverage.totalDeepClimb != 0)
            {
                newAverage.successfulDeepClimb = ((newAverage.successfulDeepClimb / newAverage.totalDeepClimb)) * 100;
            }
            if (newAverage.successfulShallowClimb != 0)
            {
                newAverage.successfulShallowClimb = ((newAverage.successfulShallowClimb / newAverage.totalShallowClimb)) * 100;
            }
            //Even More Spaget
             newAverage.totalDeepClimb = Decimal.ToInt32(tempD);
             newAverage.totalShallowClimb = Decimal.ToInt32(tempS);
            //End of Speget
            newAverage.percentMoblilitize = (newAverage.percentMoblilitize / newAverage.numMatches * 100);
            newAverage.totalPoints = newAverage.totalPoints / newAverage.numMatches;
        }
    }
}

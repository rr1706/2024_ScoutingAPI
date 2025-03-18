using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RRScout.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RRScout.Entities;
using RRScout.Helpers;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;



namespace RRScout.Controllers
{

    [ApiController]
    [Route("api/DataValidation2025")]
    public class DataValidation_2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;

        private const int autoL1Error = 0;
        private const int autoL2Error = 0;
        private const int autoL3Error = 0;
        private const int autoL4Error = 0;
        private const int teleL1Error = 1;
        private const int teleL2Error = 1;
        private const int teleL3Error = 1;
        private const int teleL4Error = 1;
        private const int processorError = 2;
        private const int netError = 2;


        private const int teleopSpeakerConst = 3;

        public DataValidation_2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

      //  GET: api/DataValidation/getTBAFlaggedMatches
       [HttpGet("getTBAFlaggedMatches")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ValidatedMatchDTO>>> GetTBAFlaggedMatches(string eventID)
        {
            try
            {
                List<ValidatedMatchDTO> validatedMatches = new List<ValidatedMatchDTO>();

                RRScout.Entities.Event selectedEvent = await Context.Events.Where(x => x.eventCode == eventID).FirstOrDefaultAsync();
                var TBAMatches = await TBAHelper.getMatchData(selectedEvent.tbaCode);

                var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID).ToListAsync();


                foreach (var tbaMatch in TBAMatches)
                {
                    if (tbaMatch.score_breakdown != null && tbaMatch.comp_level == "qm")
                    {
                        //check if match is in matchData

                        //red
                        MatchData_2025 red1 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[0])).FirstOrDefault();
                        MatchData_2025 red2 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[1])).FirstOrDefault();
                        MatchData_2025 red3 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[2])).FirstOrDefault();

                        //blue
                        MatchData_2025 blue1 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.blue.team_keys[0])).FirstOrDefault();
                        MatchData_2025 blue2 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.blue.team_keys[1])).FirstOrDefault();
                        MatchData_2025 blue3 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.blue.team_keys[2])).FirstOrDefault();

                        if (red1 != null)
                        {
                            if (ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot1) != red1.mobilitize)
                            {
                                var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot1);
                                red1.mobilitize = correctMobility;
                                red1.edited = 1;
                                await Context.SaveChangesAsync();

                            }

                            var tbaEnd = convertEndGame(tbaMatch.score_breakdown.red.endGameRobot1);
                            if (tbaEnd != red1.endClimb)
                            {
                                if (tbaEnd == "Deep" || tbaEnd == "Shallow")
                                {
                                    red1.endClimb = tbaEnd;
                                    red1.edited = 1;
                                    await Context.SaveChangesAsync();

                                }
                                else if (red1.endClimb == "Deep" || red1.endClimb == "Shallow")
                                {
                                    red1.endClimb = "No";
                                    red1.edited = 1;
                                    await Context.SaveChangesAsync();
                                }
                            }
                        }
                        if (red2 != null)
                        {
                            if (ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot2) != red2.mobilitize)
                            {
                                var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot2);
                                red2.mobilitize = correctMobility;
                                red2.edited = 1;
                                await Context.SaveChangesAsync();

                            }
                            var tbaEnd = convertEndGame(tbaMatch.score_breakdown.red.endGameRobot2);
                            if (tbaEnd != red2.endClimb)
                            {
                                if (tbaEnd == "Deep" || tbaEnd == "Shallow")
                                {
                                    red2.endClimb = tbaEnd;
                                    red2.edited = 1;
                                    await Context.SaveChangesAsync();

                                }
                                else if (red2.endClimb == "Deep" || red2.endClimb == "Shallow")
                                {
                                    red2.endClimb = "No";
                                    red2.edited = 1;
                                    await Context.SaveChangesAsync();
                                }
                            }

                        }
                        if (red3 != null)
                        {
                            if (ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot2) != red3.mobilitize)
                            {
                                var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot3);
                                red3.mobilitize = correctMobility;
                                red3.edited = 1;
                                await Context.SaveChangesAsync();

                            }
                            var tbaEnd = convertEndGame(tbaMatch.score_breakdown.red.endGameRobot3);
                            if (tbaEnd != red3.endClimb)
                            {
                                if (tbaEnd == "Deep" || tbaEnd == "Shallow")
                                {
                                    red3.endClimb = tbaEnd;
                                    red3.edited = 1;
                                    await Context.SaveChangesAsync();

                                }
                                else if (red3.endClimb == "Deep" || red3.endClimb == "Shallow")
                                {
                                    red3.endClimb = "No";
                                    red3.edited = 1;
                                    await Context.SaveChangesAsync();
                                }
                            }
                        }
                        if (blue1 != null)
                        {
                            if (ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot1) != blue1.mobilitize)
                            {
                                var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot1);
                                blue1.mobilitize = correctMobility;
                                blue1.edited = 1;
                                await Context.SaveChangesAsync();

                            }
                            var tbaEnd = convertEndGame(tbaMatch.score_breakdown.blue.endGameRobot1);
                            if (tbaEnd != blue1.endClimb)
                            {
                                if (tbaEnd == "Deep" || tbaEnd == "Shallow")
                                {
                                    blue1.endClimb = tbaEnd;
                                    blue1.edited = 1;
                                    await Context.SaveChangesAsync();

                                }
                                else if (blue1.endClimb == "Deep" || blue1.endClimb == "Shallow")
                                {
                                    blue1.endClimb = "No";
                                    blue1.edited = 1;
                                    await Context.SaveChangesAsync();
                                }
                            }
                        }
                        if (blue2 != null)
                        {
                            if (ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot2) != blue2.mobilitize)
                            {
                                var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot2);
                                blue2.mobilitize = correctMobility;
                                blue2.edited = 1;
                                await Context.SaveChangesAsync();

                            }
                            var tbaEnd = convertEndGame(tbaMatch.score_breakdown.blue.endGameRobot2);
                            if (tbaEnd != blue2.endClimb)
                            {
                                if (tbaEnd == "Deep" || tbaEnd == "Shallow")
                                {
                                    blue2.endClimb = tbaEnd;
                                    blue2.edited = 1;
                                    await Context.SaveChangesAsync();

                                }
                                else if (blue2.endClimb == "Deep" || blue2.endClimb == "Shallow")
                                {
                                    blue2.endClimb = "No";
                                    blue2.edited = 1;
                                    await Context.SaveChangesAsync();
                                }
                            }
                        }
                        if (blue3 != null)
                        {
                            if (ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot2) != blue3.mobilitize)
                            {
                                var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot3);
                                blue3.mobilitize = correctMobility;
                                blue3.edited = 1;
                                await Context.SaveChangesAsync();

                            }
                            var tbaEnd = convertEndGame(tbaMatch.score_breakdown.blue.endGameRobot3);
                            if (tbaEnd != blue3.endClimb)
                            {
                                if (tbaEnd == "Deep" || tbaEnd == "Shallow")
                                {
                                    blue3.endClimb = tbaEnd;
                                    blue3.edited = 1;
                                    await Context.SaveChangesAsync();

                                }
                                else if (blue3.endClimb == "Deep" || blue3.endClimb == "Shallow")
                                {
                                    blue3.endClimb = "No";
                                    blue3.edited = 1;
                                    await Context.SaveChangesAsync();
                                }
                            }
                        }

                        // Validate red alliance
                        if (red1 != null && red2 != null && red3 != null)
                        {
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "autoCoralL1", tbaMatch.score_breakdown.red.autoL1, autoL1Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "autoCoralL2", tbaMatch.score_breakdown.red.autoL2, autoL2Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "autoCoralL3", tbaMatch.score_breakdown.red.autoL3, autoL3Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "autoCoralL4", tbaMatch.score_breakdown.red.autoL4, autoL4Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "coralL1", tbaMatch.score_breakdown.red.teleL1, teleL1Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "coralL2", tbaMatch.score_breakdown.red.teleL2, teleL2Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "coralL3", tbaMatch.score_breakdown.red.teleL3, teleL3Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "coralL4", tbaMatch.score_breakdown.red.teleL4, teleL4Error);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "processor", tbaMatch.score_breakdown.red.wallAlgaeCount, processorError);
                            validateField(validatedMatches, tbaMatch, red1, red2, red3, "Red", "net", tbaMatch.score_breakdown.red.netAlgaeCount, netError);
                        }

                        // Validate blue alliance
                        if (blue1 != null && blue2 != null && blue3 != null)
                        {
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "autoCoralL1", tbaMatch.score_breakdown.blue.autoL1, autoL1Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "autoCoralL2", tbaMatch.score_breakdown.blue.autoL2, autoL2Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "autoCoralL3", tbaMatch.score_breakdown.blue.autoL3, autoL3Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "autoCoralL4", tbaMatch.score_breakdown.blue.autoL4, autoL4Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "coralL1", tbaMatch.score_breakdown.blue.teleL1, teleL1Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "coralL2", tbaMatch.score_breakdown.blue.teleL2, teleL2Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "coralL3", tbaMatch.score_breakdown.blue.teleL3, teleL3Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "coralL4", tbaMatch.score_breakdown.blue.teleL4, teleL4Error);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "processor", tbaMatch.score_breakdown.blue.wallAlgaeCount, processorError);
                            validateField(validatedMatches, tbaMatch, blue1, blue2, blue3, "Blue", "net", tbaMatch.score_breakdown.blue.netAlgaeCount, netError);
                        }
                    }
                }
                validatedMatches = validatedMatches.OrderBy(x => x.matchNumber).ToList();
                return Ok(validatedMatches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        //  GET: api/DataValidation/getTBAFlaggedMatches
        [HttpGet("getPredictionStandings")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<PredictionsDTO>>> getPredictionStandings(string eventID)
        {

            List<PredictionsDTO> results = new List<PredictionsDTO>();

            RRScout.Entities.Event selectedEvent = await Context.Events.Where(x => x.eventCode == eventID).FirstOrDefaultAsync();
            var TBAMatches = await TBAHelper.getMatchData(selectedEvent.tbaCode);

            var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID).ToListAsync();

            var scoutNames = matchData.Select(x => x.scoutName).Distinct().ToList();

            foreach (var scout in scoutNames)
            {
                PredictionsDTO newResult = new PredictionsDTO
                {
                    scoutName = scout,
                    score = 0,
                    numberMatches = 0,

                };
                results.Add(newResult);
            }

            foreach (var ourMatch in matchData)
            {
                var TBAMatch = TBAMatches.Where(x => x.match_number == ourMatch.matchNumber && x.comp_level == "qm").FirstOrDefault();

                if (TBAMatch is not null)
                {
                    if (ourMatch.gambleColor == TBAMatch.winning_alliance)
                    {
                        foreach (var scout in results)
                        {
                            if (scout.scoutName == ourMatch.scoutName)
                            {
                                scout.score = scout.score + ourMatch.gambleAmount;
                                scout.numberMatches = scout.numberMatches + 1;
                            }
                        }
                    }
                    else
                    {
                        foreach (var scout in results)
                        {
                            if (scout.scoutName == ourMatch.scoutName)
                            {
                                scout.score = scout.score - ourMatch.gambleAmount;
                                scout.numberMatches = scout.numberMatches + 1;
                            }
                        }
                    }
                }
            }
            results = results.OrderByDescending(x => x.numberMatches).OrderByDescending(x => x.score).ToList();
            return Ok(results);
        }
        private static void AddToValidatedMatches(List<ValidatedMatchDTO> validatedMatches, int matchNumber, string field, int currentValue, int correctValue, int team1, int team2, int team3, string matchVideo, string allianceColor)
        {
            ValidatedMatchDTO newMatch = new ValidatedMatchDTO
            {
                matchNumber = matchNumber,
                field = field,
                currentValue = currentValue,
                correctValue = correctValue,
                teamNumbers = new List<int> { team1, team2, team3 },
                matchVideo = matchVideo,
                allianceColor = allianceColor,
            };
            validatedMatches.Add(newMatch);
        }
        private static int ConvertMoblity(string mobility)
        {
            if (mobility == "Yes")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static string convertEndGame(string climb)
        {
            if (climb.Contains("Deep")){
                return "Deep";
            }else if (climb.Contains("Shallow")){
                return "Shallow";
            }
            else
            {
                return "No";
            }
        }
        private static void validateField(List<ValidatedMatchDTO> validatedMatches, TBAMatch_2025 tbaMatch, MatchData_2025 team1, MatchData_2025 team2, MatchData_2025 team3, string allianceColor, string field, int tbaValue, int errorMargin)
        {
            int ourValue = 0;
            switch (field)
            {
                case "autoCoralL1":
                    ourValue = team1.autoCoralL1 + team2.autoCoralL1 + team3.autoCoralL1;
                    break;
                case "autoCoralL2":
                    ourValue = team1.autoCoralL2 + team2.autoCoralL2 + team3.autoCoralL2;
                    break;
                case "autoCoralL3":
                    ourValue = team1.autoCoralL3 + team2.autoCoralL3 + team3.autoCoralL3;
                    break;
                case "autoCoralL4":
                    ourValue = team1.autoCoralL4 + team2.autoCoralL4 + team3.autoCoralL4;
                    break;
                case "coralL1":
                    ourValue = team1.coralL1 + team2.coralL1 + team3.coralL1;
                    break;
                case "coralL2":
                    ourValue = team1.coralL2 + team2.coralL2 + team3.coralL2;
                    break;
                case "coralL3":
                    ourValue = team1.coralL3 + team2.coralL3 + team3.coralL3;
                    break;
                case "coralL4":
                    ourValue = team1.coralL4 + team2.coralL4 + team3.coralL4;
                    break;
                case "processor":
                    ourValue = team1.autoProcessor + team2.autoProcessor + team3.autoProcessor + team1.processor + team2.processor + team3.processor;
                    break;
                case "net":
                    ourValue = team1.autoBarge + team2.autoBarge + team3.autoBarge + team1.barge + team2.barge + team3.barge;
                    break;
            }

            if (Math.Abs(ourValue - tbaValue) > errorMargin)
            {
                if (tbaMatch.videos.Count > 0)
                {
                    AddToValidatedMatches(validatedMatches, tbaMatch.match_number, field, ourValue, tbaValue, team1.teamNumber, team2.teamNumber, team3.teamNumber, tbaMatch.videos[0].key, allianceColor);
                }
            }
        }
    }

}

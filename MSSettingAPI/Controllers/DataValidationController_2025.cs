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



namespace RRScout.Controllers
{

    [ApiController]
    [Route("api/DataValidation2025")]
    public class DataValidation_2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;

        private const int autoL1Error = 1;
        private const int autoL2Error = 1;
        private const int autoL3Error = 1;
        private const int autoL4Error = 1;
        private const int teleL1Error = 1;
        private const int teleL2Error = 1;
        private const int teleL3Error = 1;
        private const int teleL4Error = 1;
        private const int processor = 2;
        private const int net = 2;

        private const int teleopSpeakerConst = 3;

        public DataValidation_2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

      //  GET: api/DataValidation/getTBAFlaggedMatches
       [HttpGet("getTBAFlaggedMatches")]
        public async Task<ActionResult<List<ValidatedMatchDTO>>> GetTBAFlaggedMatches(string eventID)
        {

            List<ValidatedMatchDTO> validatedMatches = new List<ValidatedMatchDTO>();

            RRScout.Entities.Event selectedEvent = await Context.Events.Where(x => x.eventCode == eventID).FirstOrDefaultAsync();
            var TBAMatches = await TBAHelper.getMatchData(selectedEvent.tbaCode);

            var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID).ToListAsync();


            //correct mobilitize and end game automatically




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
                            Context.SaveChangesAsync();

                        }

                        //need to confirm this logic is correct
                        var tbaEnd = convertEndGame(tbaMatch.score_breakdown.red.endGameRobot1);
                        if (tbaEnd != red1.endClimb)
                        {
                            if(tbaEnd == "Deep" || tbaEnd == "Shallow")
                            {
                                red1.endClimb = tbaEnd;
                                red1.edited = 1;
                                Context.SaveChangesAsync();

                            }
                            else if (red1.endClimb == "Deep" || red1.endClimb == "Shallow")
                            {
                                red1.endClimb = "No";
                                red1.edited = 1;
                                Context.SaveChangesAsync();
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
                            Context.SaveChangesAsync();

                        }
                    }
                    if (red3 != null)
                    {
                        if (ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot2) != red3.mobilitize)
                        {
                            var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot3);
                            red3.mobilitize = correctMobility;
                            red3.edited = 1;
                            Context.SaveChangesAsync();

                        }
                    }
                    if (blue1 != null)
                    {
                        if (ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot1) != blue1.mobilitize)
                        {
                            var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot1);
                            blue1.mobilitize = correctMobility;
                            blue1.edited = 1;
                            Context.SaveChangesAsync();

                        }
                    }
                    if (blue2 != null)
                    {
                        if (ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot2) != blue2.mobilitize)
                        {
                            var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot2);
                            blue2.mobilitize = correctMobility;
                            blue2.edited = 1;
                            Context.SaveChangesAsync();

                        }
                    }
                    if (blue3 != null)
                    {
                        if (ConvertMoblity(tbaMatch.score_breakdown.red.autoLineRobot2) != blue3.mobilitize)
                        {
                            var correctMobility = ConvertMoblity(tbaMatch.score_breakdown.blue.autoLineRobot3);
                            blue3.mobilitize = correctMobility;
                            blue3.edited = 1;
                            Context.SaveChangesAsync();

                        }
                    }

                    //red
                    if (red1 != null && red2 != null && red3 != null)
                    {
                        var ourAutoCoralL1 = red1.autoCoralL1 + red2.autoCoralL1 + red3.autoCoralL1;
                        if (Math.Abs(ourAutoCoralL1 - tbaMatch.score_breakdown.red.autoL1) > autoL1Error)
                        {
                            AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "autoCoralL1", ourAutoCoralL1, tbaMatch.score_breakdown.red.autoL1, red1.teamNumber, red2.teamNumber, red3.teamNumber, tbaMatch.videos[0].key, "Red");

                        }
                    }

                    //blue
                    if (blue1 != null && blue2 != null && blue3 != null)
                    {
                        var ourAutoCoralL1 = blue1.autoCoralL1 + blue2.autoCoralL1 + blue3.autoCoralL1;
                        if (Math.Abs(ourAutoCoralL1 - tbaMatch.score_breakdown.red.autoL1) > autoL1Error)
                        {
                            AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "autoCoralL1", ourAutoCoralL1, tbaMatch.score_breakdown.blue.autoL1, blue1.teamNumber, blue2.teamNumber, blue3.teamNumber, tbaMatch.videos[0].key, "Blue");

                        }
                    }
                }
            }
            validatedMatches = validatedMatches.OrderBy(x => x.matchNumber).ToList();
            return Ok(validatedMatches);

        }
        //  GET: api/DataValidation/getTBAFlaggedMatches
        [HttpGet("getPredictionStandings")]
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
    }

}

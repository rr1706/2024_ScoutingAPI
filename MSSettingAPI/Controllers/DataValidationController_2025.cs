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



namespace RRScout.Controllers
{

    [ApiController]
    [Route("api/DataValidation2025")]
    public class DataValidation_2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;

        private const int autoSpeakerConst = 2;
        private const int teleopSpeakerConst = 3;

        public DataValidation_2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

      //  GET: api/DataValidation/getTBAFlaggedMatches
       [HttpGet("getTBAFlaggedMatches")]
        public async Task<ActionResult<List<TBAMatch_2025>>> GetTBAFlaggedMatches(string eventID)
        {

            List<ValidatedMatchDTO> validatedMatches = new List<ValidatedMatchDTO>();


            var TBAMatches = await TBAHelper.getMatchData(eventID);

            return Ok(TBAMatches);

            var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID).ToListAsync();




            //foreach (var tbaMatch in TBAMatches)
            //{
            //    if (tbaMatch.score_breakdown != null && tbaMatch.comp_level == "qm")
            //    {
            //        //check if match is in matchData

            //        //red
            //        MatchData_2025 red1 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[0])).FirstOrDefault();
            //        MatchData_2025 red2 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[1])).FirstOrDefault();
            //        MatchData_2025 red3 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[2])).FirstOrDefault();

            //        //blue
            //        MatchData_2025 blue1 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.blue.team_keys[0])).FirstOrDefault();
            //        MatchData_2025 blue2 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.blue.team_keys[1])).FirstOrDefault();
            //        MatchData_2025 blue3 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.blue.team_keys[2])).FirstOrDefault();

            //        //Checking Autospeaker + TeleopSpeaker

            //        //red
            //        if (red1 != null && red2 != null && red3 != null)
            //        {
            //            var ourRedAutoSpeaker = red1.autoSpeaker + red2.autoSpeaker + red3.autoSpeaker;
            //            var ourRedTeleopSpeaker = red1.teleSpeaker + red2.teleSpeaker + red3.teleSpeaker;
            //            if (Math.Abs(ourRedAutoSpeaker - tbaMatch.score_breakdown.red.autoSpeakerNoteCount) > autoSpeakerConst)
            //            {
            //                AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "autoSpeaker", ourRedAutoSpeaker, tbaMatch.score_breakdown.red.autoSpeakerNoteCount, red1.teamNumber, red2.teamNumber, red3.teamNumber, tbaMatch.videos[0].key, "Red");

            //            }

            //            if (Math.Abs(ourRedTeleopSpeaker - tbaMatch.score_breakdown.red.teleopSpeakerNoteCount) > teleopSpeakerConst)
            //            {
            //                AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "teleSpeaker", ourRedTeleopSpeaker, tbaMatch.score_breakdown.red.teleopSpeakerNoteCount, red1.teamNumber, red2.teamNumber, red3.teamNumber, tbaMatch.videos[0].key, "Red");
            //            }
            //        }

            //        //blue
            //        if (blue1 != null && blue2 != null && blue3 != null)
            //        {
            //            var ourBlueAutoSpeaker = blue1.autoSpeaker + blue2.autoSpeaker + blue3.autoSpeaker;
            //            var ourBlueTeleopSpeaker = blue1.teleSpeaker + blue2.teleSpeaker + blue3.teleSpeaker;
            //            if (Math.Abs(ourBlueAutoSpeaker - tbaMatch.score_breakdown.blue.autoSpeakerNoteCount) > autoSpeakerConst)
            //            {
            //                AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "autoSpeaker", ourBlueAutoSpeaker, tbaMatch.score_breakdown.blue.autoSpeakerNoteCount, blue1.teamNumber, blue2.teamNumber, blue3.teamNumber, tbaMatch.videos[0].key, "Blue");

            //            }

            //            if (Math.Abs(ourBlueTeleopSpeaker - tbaMatch.score_breakdown.blue.teleopSpeakerNoteCount) > teleopSpeakerConst)
            //            {
            //                AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "teleSpeaker", ourBlueTeleopSpeaker, tbaMatch.score_breakdown.blue.teleopSpeakerNoteCount, blue1.teamNumber, blue2.teamNumber, blue3.teamNumber, tbaMatch.videos[0].key, "Blue");
            //            }
            //        }
            //    }
            //}
            validatedMatches = validatedMatches.OrderBy(x => x.matchNumber).ToList();
            return Ok(validatedMatches);

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
                allianceColor = allianceColor + " Alliance"
            };
            validatedMatches.Add(newMatch);
        }

        
    }

}

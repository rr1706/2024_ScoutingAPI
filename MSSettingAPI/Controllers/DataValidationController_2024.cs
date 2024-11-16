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



namespace RRScout.Controllers
{

    [ApiController]
    [Route("api/DataValidation2024")]
    public class DataValidation_2024 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;

        private const int autoSpeakerConst = 2;

        public DataValidation_2024(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        // GET: api/DataValidation/getTBAFlaggedMatches
        [HttpGet("getTBAFlaggedMatches")]
        public async Task<ActionResult<List<ValidatedMatchDTO>>> GetTBAFlaggedMatches(string eventID)
        {
            var httpClient = new HttpClient();

            List<ValidatedMatchDTO> validatedMatches = new List<ValidatedMatchDTO>();


            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.thebluealliance.com/api/v3/event/" + eventID + "/matches");
            request.Headers.Add("X-TBA-Auth-Key", "e6O1xGxIT7zsDwNUM1gAb7cNsH71EZ4JhyyvAkBwiw1qDRcEvhsW8CBUCkXxCVA8");

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error calling TBA API");
            }

            var TBAMatches = JsonSerializer.Deserialize<List<TBAMatch_2024>>(await response.Content.ReadAsStringAsync());

            var matchData = await Context.MatchData_2024.Where(x => x.eventCode == eventID).ToListAsync();

            //Remove 'FRC' from team number
            foreach (var tbaMatch in TBAMatches)
            {
                if (tbaMatch.score_breakdown != null && tbaMatch.comp_level == "qm")
                {
                    tbaMatch.alliances.red.team_keys[0] = tbaMatch.alliances.red.team_keys[0].Substring(3);
                    tbaMatch.alliances.red.team_keys[1] = tbaMatch.alliances.red.team_keys[1].Substring(3);
                    tbaMatch.alliances.red.team_keys[2] = tbaMatch.alliances.red.team_keys[2].Substring(3);
                    tbaMatch.alliances.blue.team_keys[0] = tbaMatch.alliances.blue.team_keys[0].Substring(3);
                    tbaMatch.alliances.blue.team_keys[1] = tbaMatch.alliances.blue.team_keys[1].Substring(3);
                    tbaMatch.alliances.blue.team_keys[2] = tbaMatch.alliances.blue.team_keys[2].Substring(3);
                }
            }


            foreach (var tbaMatch in TBAMatches)
            {
                if (tbaMatch.score_breakdown != null && tbaMatch.comp_level == "qm")
                {
                    //check if match is in matchData
                    var red1 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[0])).FirstOrDefault();
                    var red2 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[1])).FirstOrDefault();
                    var red3 = matchData.Where(x => x.matchNumber == tbaMatch.match_number && x.teamNumber == int.Parse(tbaMatch.alliances.red.team_keys[2])).FirstOrDefault();

                    if (red1 != null && red2 != null && red3 != null)
                    {
                        var ourAutoSpeaker = red1.autoSpeaker + red2.autoSpeaker + red3.autoSpeaker;
                        if (Math.Abs(ourAutoSpeaker - tbaMatch.score_breakdown.red.autoSpeakerNoteCount) > autoSpeakerConst)
                        {
                            AddToValidatedMatches(validatedMatches, tbaMatch.match_number, "autoSpeaker", ourAutoSpeaker, tbaMatch.score_breakdown.red.autoSpeakerNoteCount, red1.teamNumber, red2.teamNumber, red3.teamNumber, tbaMatch.videos[0].key);

                        }
                    }
                }
            }
            return Ok(validatedMatches);

        }
        private static void AddToValidatedMatches(List<ValidatedMatchDTO> validatedMatches, int matchNumber, string field, int currentValue, int correctValue, int team1, int team2, int team3, string matchVideo)
        {
            ValidatedMatchDTO newMatch = new ValidatedMatchDTO
            {
                matchNumber = matchNumber,
                field = field,
                currentValue = currentValue,
                correctValue = correctValue,
                teamNumbers = new List<int> { team1, team2, team3 },
                matchVideo = matchVideo
            };
            validatedMatches.Add(newMatch);
        }
    }

}

using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRScout.DTOs;
using RRScout.Entities;
using System;
using System.Globalization;

namespace RRScout.Controllers
{
    [Route("api/matchdata2024")]
    [ApiController]
    public class MatchData2024 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public MatchData2024(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpPost("savedata")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SaveData(List<MatchDataDTO_2024> matchDTO)
        {
            try
            {
                var newMatches = mapper.Map<List<MatchData_2024>>(matchDTO);

                var exisitingMatches = await Context.MatchData_2024.Where(x => x.eventCode == newMatches[0].eventCode).ToListAsync();

                foreach (MatchData_2024 match in newMatches)
                {
                    var result = exisitingMatches.Where(x => x.eventCode == match.eventCode && x.matchNumber == match.matchNumber && x.teamNumber == match.teamNumber).ToList();
                    if (result.Count == 0)
                    {
                        await Context.MatchData_2024.AddAsync(match);
                    }
                }
                await Context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("event")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchData_2024>>> Get(string eventID)
        {
            var matchData = await Context.MatchData_2024.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(matchData);
        }
        [HttpGet("getbyteam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchDataDTO_2024>>> GetTeamMatchData(string eventID, int teamNumber)
        {
            var matchData = await Context.MatchData_2024.Where(x => x.eventCode == eventID && x.teamNumber == teamNumber).ToListAsync();
            return Ok(mapper.Map<List<MatchDataDTO_2024>>(matchData));
        }
    }


}

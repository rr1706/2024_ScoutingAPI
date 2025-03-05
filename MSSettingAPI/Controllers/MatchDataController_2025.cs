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
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RRScout.Controllers
{
    [Route("api/matchdata2025")]
    [ApiController]
    public class MatchData2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public MatchData2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpPost("savedata")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SaveData(List<MatchDataDTO_2025> matchDTO)
        {
            try
            {
                var newMatches = mapper.Map<List<MatchData_2025>>(matchDTO);

                var exisitingMatches = await Context.MatchData_2025.Where(x => x.eventCode == newMatches[0].eventCode).ToListAsync();

                foreach (MatchData_2025 match in newMatches)
                {
                    var result = exisitingMatches.Where(x => x.eventCode == match.eventCode && x.matchNumber == match.matchNumber && x.teamNumber == match.teamNumber).ToList();
                    if (result.Count == 0)
                    {
                        await Context.MatchData_2025
                            .AddAsync(match);
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchData_2025>>> Get(string eventID)
        {
            var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(matchData);
        }
        [HttpGet("getbyteam")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchDataDTO_2025>>> GetTeamMatchData(string eventID, int teamNumber)
        {
            var matchData = await Context.MatchData_2025.Where(x => x.eventCode == eventID && x.teamNumber == teamNumber).ToListAsync();
            return Ok(mapper.Map<List<MatchDataDTO_2025>>(matchData));
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("sendignore/{matchID}/{ignore}")]
        public async Task<ActionResult<List<MatchDataDTO_2025>>> SendIgnore(int matchID, int ignore)
        {
            var match = await Context.MatchData_2025.Where(x => x.id == matchID).FirstOrDefaultAsync();
            if (match != null)
            {
                match.ignore = ignore;
                Context.SaveChanges();
            }
            return Ok();
        }
    

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("updatematchbymatch")]
    public async Task<ActionResult<List<MatchDataDTO_2025>>> updateMatchByMatch(MatchDataDTO_2025 matchData)
    {
        var match = await Context.MatchData_2025.Where(x => x.id == matchData.id).FirstOrDefaultAsync();
        if (match != null)
        {
                match.coralL1 = matchData.coralL1;
                match.coralL2 = matchData.coralL2;
                match.coralL3 = matchData.coralL3;
                match.coralL4 = matchData.coralL4;
                match.autoCoralL1 = matchData.autoCoralL1;
                match.autoCoralL2 = matchData.autoCoralL2;
                match.autoCoralL3 = matchData.autoCoralL3;
                match.autoCoralL4 = matchData.autoCoralL4;
                match.processor = matchData.processor;
                match.autoProcessor = matchData.autoProcessor;
                match.endClimb = matchData.endClimb;
                match.groundAlgae = matchData.groundAlgae;
                match.autoGroundAlgae = matchData.autoGroundAlgae;
                match.reefAlgae = matchData.reefAlgae;
                match.autoReefAlgae = matchData.autoReefAlgae;
                match.barge = matchData.barge;
                match.autoBarge = matchData.autoBarge;
                match.defence = matchData.defence;
                match.defended = matchData.defended;
                match.mobilitize = matchData.mobilitize;
                match.doNotPick = matchData.doNotPick;
                match.edited = 1;

                Context.SaveChanges();
        }
        return Ok();
    }
}

}

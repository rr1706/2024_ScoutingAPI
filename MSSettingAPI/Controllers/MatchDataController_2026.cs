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
    [Route("api/matchdata2026")]
    [ApiController]
    public class MatchData2026 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public MatchData2026(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpPost("savedata")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SaveData(List<MatchDataDTO_2026> matchDTO)
        {
            try
            {
                var newMatchDTO = matchDTO.GroupBy(x => x.matchNumber & x.teamNumber).Select(x => x.First()).ToList();

                var newMatches = mapper.Map<List<MatchData_2026>>(newMatchDTO);

                var exisitingMatches = await Context.MatchData_2026.Where(x => x.eventCode == newMatches[0].eventCode).ToListAsync();

                foreach (MatchData_2026 match in newMatches)
                {
                    var result = exisitingMatches.Where(x => x.eventCode == match.eventCode && x.matchNumber == match.matchNumber && x.teamNumber == match.teamNumber).ToList();
                    if (result.Count == 0)
                    {
                        await Context.MatchData_2026
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchData_2026>>> Getfg(string eventID)
        {
            var matchData = await Context.MatchData_2026.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(matchData);
        }
        [HttpGet("getbyteam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchDataDTO_2026>>> GetTeamMatchData(string eventID, int teamNumber)
        {
            var matchData = await Context.MatchData_2026.Where(x => x.eventCode == eventID && x.teamNumber == teamNumber).ToListAsync();
            return Ok(mapper.Map<List<MatchDataDTO_2026>>(matchData));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("sendignore/{matchID}/{ignore}")]
        public async Task<ActionResult<List<MatchDataDTO_2026>>> SendIgnore(int matchID, bool ignore)
        {
            var match = await Context.MatchData_2026.Where(x => x.id == matchID).FirstOrDefaultAsync();
            if (match != null)
            {
                match.ignore = ignore;
                Context.SaveChanges();
            }
            return Ok();
        }
    

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("updatematchbymatch")]
    public async Task<ActionResult<List<MatchDataDTO_2026>>> updateMatchByMatch(MatchDataDTO_2026 matchData)
    {
        var match = await Context.MatchData_2026.Where(x => x.id == matchData.id).FirstOrDefaultAsync();
        if (match != null)
        {
                match.autoClimb = matchData.autoClimb;
                match.endClimb = matchData.endClimb;
                match.autoFuelScored = matchData.autoFuelScored;
                match.teleFuelScored = matchData.teleFuelScored;
                match.autoFuelFed = matchData.autoFuelFed;
                match.fuelSourceMidfield = matchData.fuelSourceMidfield;
                match.fuelSourceDepot = matchData.fuelSourceDepot;
                match.fuelSourceHP = matchData.fuelSourceHP;
                match.fuelSourcePreLoadOnly = matchData.fuelSourcePreLoadOnly;
                match.shotAccuracy = matchData.shotAccuracy;
                match.shotRate = matchData.shotRate;
                match.defense = matchData.defense;
                match.ignore = matchData.ignore;
                match.edited = true; 
                if (matchData.endClimb == "top")
                {
                    match.endClimbPoints = 30;
                }
                else if (matchData.endClimb == "middle")
                {
                    match.endClimbPoints = 20;
                }
                else if (matchData.endClimb == "bottom")
                {
                    match.endClimbPoints = 10;
                }
                else { match.endClimbPoints = 0; }
                Context.SaveChanges();
        }
        return Ok();
    }
}

}

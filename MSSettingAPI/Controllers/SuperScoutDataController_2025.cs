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
    [Route("api/superscoutdata2025")]
    [ApiController]
    public class SuperScoutData2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public SuperScoutData2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpPost("savedata")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SaveData(SuperScoutDataDTO_2025 superScoutData)
        {
            try
            {
                var newSuperData = mapper.Map<SuperScoutData_2025>(superScoutData);

                var exisitingMatches = await Context.SuperScoutData_2025.Where(x => x.eventCode == newSuperData.eventCode).ToListAsync();

                await Context.SuperScoutData_2025.AddAsync(newSuperData);

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
        public async Task<ActionResult<List<SuperScoutDataDTO_2025>>> Get(string eventID)
        {
            var matchData = await Context.SuperScoutData_2025.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(mapper.Map<List<SuperScoutDataDTO_2025>>(matchData));
        }
        [HttpGet("getbyteam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<SuperScoutDataDTO_2025>>> GetTeamMatchData(string eventID, int teamNumber)
        {
            var matchData = await Context.SuperScoutData_2025.Where(x => x.eventCode == eventID && x.teamNumber == teamNumber).ToListAsync();
            return Ok(mapper.Map<List<SuperScoutDataDTO_2025>>(matchData));
        }

        //get by team and type?
    }

}

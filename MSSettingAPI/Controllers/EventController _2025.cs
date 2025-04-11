using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRScout.DTOs;
using RRScout.Entities;
using RRScout.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RRScout.Controllers
{
    [Route("api/event2025")]
    [ApiController]
    public class Event2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public Event2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpGet("getevents")]
        public async Task<ActionResult<List<EventDTO>>> GetEvents()
        {
            try
            {
                var events = await Context.Events.Where(x => x.year == 2025).ToListAsync();
                return Ok(mapper.Map<List<EventDTO>>(events).OrderBy(x => x.eventName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("saveevent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<EventDTO>>> PostEvent(RRScout.Entities.Event eventDTO)
        {
            try
            {
                RRScout.Entities.Event newEvent = new RRScout.Entities.Event();
                newEvent.eventName = eventDTO.eventName;
                newEvent.eventCode = eventDTO.eventCode;
                newEvent.tbaCode = eventDTO.tbaCode;
                newEvent.year = eventDTO.year;
                await Context.Events.AddAsync(newEvent);
                await Context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getteamList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<int>>> GetTeamList(string eventID)
        {
            var teamList = await Context.MatchData_2025.Where(x => x.eventCode == eventID).Select(x => x.teamNumber).Distinct().ToListAsync();
            return Ok(teamList.OrderBy(x => x));
        }

        [HttpGet("getteamname")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> getTeamName(string eventID, int teamNum)
        {
            var teamName = await Context.TeamNames.Where(x => x.eventCode == eventID && x.teamNumber == teamNum).Select(x => x.teamName).SingleOrDefaultAsync();
            if(teamName != null)
            {
                return Ok(teamName);
            }
            return "";
        }

        [HttpPost("saveschedule")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SaveSchedule(List<MatchScheduleDTO> scheduleDTO)
        {
            try
            {
                var matches = mapper.Map<List<MatchSchedule>>(scheduleDTO);

                var exisitingMatches = await Context.MatchSchedule.Where(x => x.eventCode == matches[0].eventCode).ToListAsync();
                if (exisitingMatches != null)
                {
                    Context.MatchSchedule.RemoveRange(exisitingMatches);
                }

                await Context.MatchSchedule.AddRangeAsync(matches);
                await Context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getmatchschedule")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MatchScheduleDTO>>> GetMatchSchedule(string eventID)
        {
            var matchSchedule = await Context.MatchSchedule
                .Where(x => x.eventCode == eventID)
                .OrderBy(x => x.matchNumber)
                .ToListAsync();

            return Ok(mapper.Map<List<MatchScheduleDTO>>(matchSchedule));
        }
        [HttpGet("loadMatchScheduleTBA")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> loadMatchScheduleTBA(string eventID)
        {
            try
            {
                RRScout.Entities.Event selectedEvent = await Context.Events.Where(x => x.eventCode == eventID).FirstOrDefaultAsync();
                var matches = await TBAHelper.getAllMatches(selectedEvent.tbaCode);

                if (matches == null || !matches.Any())
                {
                    return NotFound("No matches found for the specified event.");
                }

                var matchSchedules = new List<MatchSchedule>();

                foreach (var match in matches)
                {
                    if (match.alliances != null && match.comp_level == "qm")
                    {
                        var matchSchedule = new MatchSchedule
                        {
                            eventCode = eventID,
                            matchNumber = match.match_number,
                            red1 = int.Parse(match.alliances.red.team_keys[0]),
                            red2 = int.Parse(match.alliances.red.team_keys[1]),
                            red3 = int.Parse(match.alliances.red.team_keys[2]),
                            blue1 = int.Parse(match.alliances.blue.team_keys[0]),
                            blue2 = int.Parse(match.alliances.blue.team_keys[1]),
                            blue3 = int.Parse(match.alliances.blue.team_keys[2])
                        };

                        matchSchedules.Add(matchSchedule);
                    }
                }

                var existingMatches = await Context.MatchSchedule.Where(x => x.eventCode == eventID).ToListAsync();
                if (existingMatches != null)
                {
                    Context.MatchSchedule.RemoveRange(existingMatches);
                }

                await Context.MatchSchedule.AddRangeAsync(matchSchedules);
                await Context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRScout.DTOs;
using RRScout.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RRScout.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class Event : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        public Event(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpGet("getevents")]
        public async Task<ActionResult<List<EventDTO>>> GetEvents()
        {
            var events = await Context.Events.ToListAsync();
            return Ok(mapper.Map<List<EventDTO>>(events).OrderBy(x => x.eventName));
        }

        [HttpPost("saveevent")]
        public async Task<ActionResult<List<EventDTO>>> PostEvent(RRScout.Entities.Event eventDTO )
        {
            try
            {
                RRScout.Entities.Event newEvent = new RRScout.Entities.Event();
                newEvent.eventName = eventDTO.eventName;
                newEvent.eventCode = eventDTO.eventCode;
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
        public async Task<ActionResult<List<int>>> GetTeamList(string eventID)
        {
            var teamList = await Context.MatchData_2023.Where(x => x.eventCode == eventID).Select(x => x.teamNumber).Distinct().ToListAsync();
            return Ok(teamList.OrderBy(x => x));
        }

        [HttpPost("saveschedule")]
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
        public async Task<ActionResult<List<MatchScheduleDTO>>> GetMatchSchedule(string eventID)
        {
            var matchSchedule = await Context.MatchSchedule.Where(x => x.eventCode == eventID).ToListAsync();
            return Ok(mapper.Map<List<MatchScheduleDTO>>(matchSchedule));
        }
    }
}

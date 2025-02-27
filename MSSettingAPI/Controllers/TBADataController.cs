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
    [Route("api/TBAData")]
    public class TBAData_2025 : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;

        private const int autoSpeakerConst = 2;
        private const int teleopSpeakerConst = 3;

        public TBAData_2025(ApplicationDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

       [HttpGet("getAllMatches")]
        public async Task<ActionResult<List<TBAMatch_2025>>> GetAllMatches()
        {
            var events = await TBAHelper.getAllEvents();

            var TBAMatches = new List<TBAMatch_2025>();


            foreach (var comp in events)
            {
                if (DateTime.TryParse(comp.start_date, out DateTime startDate) && startDate < DateTime.Now)
                {
                    TBAMatches.AddRange(await TBAHelper.getMatchData("2025"+comp.event_code));
                }
            }

            return Ok(TBAMatches);

        }
        
    }

}

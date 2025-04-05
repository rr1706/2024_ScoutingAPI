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

        [HttpGet("getMatchVideos")]
        public async Task<ActionResult<List<TBAMatch_2025>>> getMatchVideos(string eventID)
        {
            RRScout.Entities.Event selectedEvent = await Context.Events.Where(x => x.eventCode == eventID).FirstOrDefaultAsync();
            var TBAMatches = await TBAHelper.getMatchData(selectedEvent.tbaCode);

            List<MatchVideoDTO> videos = new List<MatchVideoDTO>();

            foreach (var match in TBAMatches)
            {
                if (match.videos.Count > 0 && match.comp_level == "qm")
                {
                    MatchVideoDTO newVideo = new MatchVideoDTO();
                    newVideo.matchNumber = match.match_number;
                    newVideo.video = match.videos[0].key;
                    videos.Add(newVideo);
                }
            }
            return Ok(videos);
        }

    }

}

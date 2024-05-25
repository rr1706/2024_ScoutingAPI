using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Route("api/picklist")]
    [ApiController]
    public class Picklist : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        public Picklist(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.Context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet("getOrder")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<PicklistOrderDTO>>> Get(string eventID)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                var order = await Context.PicklistOrder.Where(x => x.eventCode == eventID && x.email == email).OrderBy(x => x.order).ToListAsync();
                return Ok(mapper.Map<List<PicklistOrderDTO>>(order));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("save")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> save(PicklistOrderDTO[] teams)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;

                foreach (var team in teams)
                {
                    var entity = await Context.PicklistOrder.FirstOrDefaultAsync(x => x.teamNumber == team.teamNumber && x.eventCode == team.eventCode && x.email == email);
                    if (entity != null) { 
                        entity.order = team.order;
                        entity.isDNPed = team.isDNPed;
                        await Context.SaveChangesAsync();
                    }
                    else
                    {
                        PicklistOrder newTeam = new PicklistOrder();
                        newTeam.teamNumber = team.teamNumber;
                        newTeam.eventCode = team.eventCode;
                        newTeam.order = team.order;
                        newTeam.email = email;
                        newTeam.isDNPed = team.isDNPed;
                        await Context.PicklistOrder.AddAsync(newTeam);
                        await Context.SaveChangesAsync();
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }


}

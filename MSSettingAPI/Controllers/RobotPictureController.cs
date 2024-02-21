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
    [Route("api/robotpicture")]
    [ApiController]
    public class RobotPicture : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        public RobotPicture(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            this.Context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpPost("{eventCode}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UploadPhotos(string eventCode, [FromForm] IFormFile photo)
        {
            try
            {
                var newPicture = new RobotPhoto();
                newPicture.eventCode = eventCode;
                newPicture.teamNumber = Int32.Parse(Path.GetFileNameWithoutExtension(photo.FileName));

                var existingPicture = await Context.RobotPhotos.SingleOrDefaultAsync(x => x.teamNumber == newPicture.teamNumber && x.eventCode == eventCode);

                if (existingPicture is not null)
                {
                    newPicture.picture = await fileStorageService.EditFile(eventCode, photo, existingPicture.picture);
                    Context.Remove(existingPicture);
                    Context.Add(newPicture);
                }
                else
                {
                    newPicture.picture = await fileStorageService.SaveFile(eventCode, photo);
                    Context.Add(newPicture);
                }
                await Context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{eventCode}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeletePhotos(string eventCode)
        {
            try
            {

                var pictures = await Context.RobotPhotos.Where(x => x.eventCode == eventCode).ToListAsync();

                foreach (var picture in pictures)
                {
                    await fileStorageService.DeleteFile(picture.picture, eventCode);
                    Context.Remove(picture);
                }
                await Context.SaveChangesAsync();
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> getPhoto(string eventCode, int teamNumber)
        {
            var picture = await Context.RobotPhotos.SingleOrDefaultAsync(x => x.teamNumber == teamNumber && x.eventCode == eventCode);

            if (picture is not null)
            {
                return Ok(picture.picture);
            }
            return NoContent();
        }
    }
}

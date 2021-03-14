using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusicCore.Models;
using MyMusicCore.Services;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _serviceArtist;
        private readonly IMapper _mapperService;

        public ArtistController(IArtistService serviceArtist, IMapper mapperService)
        {
            _serviceArtist = serviceArtist;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ArtistResource>>> GETAllArtist()
        {
            try
            {
                IEnumerable<Artist> artists = await _serviceArtist.GetAllArtists();
                IEnumerable<ArtistResource> artistResources = _mapperService.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);
                return Ok(artistResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistResource>> GETArtistById(int id)
        {
            try
            {
                Artist artist = await _serviceArtist.GetArtistById(id);
                if (artist == null) return BadRequest("cet artist n'existe pas");
                ArtistResource artistResource = _mapperService.Map<Artist, ArtistResource>(artist);
                return Ok(artistResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<ArtistResource>> CreateArist(SaveArtistResource saveArtistResource)
        {
            try
            {
                // validation
                SaveArtistResourceValidator validation = new SaveArtistResourceValidator();
                ValidationResult validationResult = await validation.ValidateAsync(saveArtistResource);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
                // mappage view to db
                Artist artist = _mapperService.Map<SaveArtistResource, Artist>(saveArtistResource);
                // Creation artist
                Artist artistNew = await _serviceArtist.CreateArtist(artist);
                // mappage view to db
                ArtistResource artistResource = _mapperService.Map<Artist, ArtistResource>(artistNew);
                return Ok(artistResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistResource>> UpdateArist(int id, SaveArtistResource saveArtistResource)
        {
            try
            {
                // Get arist by ID
                Artist artistUpdate = await _serviceArtist.GetArtistById(id);
                if (artistUpdate == null) return NotFound();
                // validation
                SaveArtistResourceValidator validation = new SaveArtistResourceValidator();
                ValidationResult validationResult = await validation.ValidateAsync(saveArtistResource);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
                //mappage
                Artist artist = _mapperService.Map<SaveArtistResource, Artist>(saveArtistResource);
                // update Artist
                await _serviceArtist.UpdateArtist(artistUpdate, artist);
                //get artistBy id
                Artist artistNew = await _serviceArtist.GetArtistById(id);
                /// mappage
                ArtistResource artisrNewResource = _mapperService.Map<Artist, ArtistResource>(artistNew);

                return Ok(artisrNewResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtist(int id)
        {
            try
            {
                var artist = await _serviceArtist.GetArtistById(id);
                if (artist == null) return NotFound();
                await _serviceArtist.DeleteArtist(artist);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
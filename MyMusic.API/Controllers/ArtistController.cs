using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
            var artists = await _serviceArtist.GetAllArtists();
            var artistResources= _mapperService.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);
            return Ok(artistResources);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistResource>> GETArtistById(int id)
        {
            try
            {
                var artist = await _serviceArtist.GetArtistById(id);
                if (artist == null) return BadRequest("cet artist n'existe pas");
                var artistResource = _mapperService.Map<Artist, ArtistResource>(artist);
                return Ok(artistResource);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("")]
        public async Task<ActionResult<ArtistResource>> CreateArist(SaveArtistResource saveArtistResource)
        {
            // validation
            var validation = new SaveArtistResourceValidator();
            var validationResult = await validation.ValidateAsync(saveArtistResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            // mappage
            var artist = _mapperService.Map<SaveArtistResource,Artist>(saveArtistResource);
            // Creation artist
            var artistNew = await _serviceArtist.CreateArtist(artist);
            // mappage
            var artistResource= _mapperService.Map<Artist,ArtistResource >(artistNew);
            return Ok(artistResource);
            
        }
        [HttpPut("")]
        public async Task<ActionResult<ArtistResource>> UpdateArist(int id,SaveArtistResource saveArtistResource)
        {
            // validation
            var validation = new SaveArtistResourceValidator();
            var validationResult = await validation.ValidateAsync(saveArtistResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            // Get arist by ID
            var artistUpdate = await _serviceArtist.GetArtistById(id);
            if (artistUpdate == null) return NotFound();
            //mappage
            var artist= _mapperService.Map<SaveArtistResource, Artist>(saveArtistResource);
            // update Artist
            await _serviceArtist.UpdateArtist(artistUpdate, artist);
            //get artistBy id
            var artistNew = await _serviceArtist.GetArtistById(id);
            /// mappage
            var artisrNewResource= _mapperService.Map<Artist, ArtistResource>(artistNew);

            return Ok(artisrNewResource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtist(int id)
        {
            var artist = await _serviceArtist.GetArtistById(id);
            if (artist == null) return NotFound();
            await _serviceArtist.DeleteArtist(artist);
            return NoContent();
        }


    }
}
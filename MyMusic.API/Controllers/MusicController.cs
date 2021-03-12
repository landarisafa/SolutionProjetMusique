using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusicCore.Models;
using MyMusicCore.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IArtistService _artistService;
        private readonly IMapper _mapperService;

        public MusicController(IMusicService musicService, IArtistService artistService, IMapper mapperService)
        {
            _musicService = musicService;
            _artistService = artistService;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusic()
        {
            try
            {
                var musics = await _musicService.GetAllWithArtist();
                //return Ok(musics);
                var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
                return Ok(musicResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicResource>> GetMusicById(int id)
        {
            try
            {
                var music = await _musicService.GetMusicById(id);
                if (music == null) return NotFound();
                var musicResource = _mapperService.Map<Music, MusicResource>(music);
                return Ok(musicResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<MusicResource>> CreateMusic(SaveMusicResource saveMusicResource)
        {
            //GET Current user
            var userId = User.Identity.Name;
            // Validation
            var validation = new SaveMusicResourceValidator();
            var validationResult = await validation.ValidateAsync(saveMusicResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            // mappage
            var music = _mapperService.Map<SaveMusicResource, Music>(saveMusicResource);
            // Creation de music
            var newMusic = await _musicService.CreateMusic(music);
            // mappage
            var musicResource = _mapperService.Map<Music, MusicResource>(newMusic);
            return Ok(musicResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MusicResource>> UpdateMusic(int id, SaveMusicResource saveMusicResource)
        {
            /// validation
            var validation = new SaveMusicResourceValidator();
            var validationResult = await validation.ValidateAsync(saveMusicResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // si la music existe depuis le id

            var musicUpdate = await _musicService.GetMusicById(id);
            if (musicUpdate == null) return BadRequest("la music n'existe pas ");
            var music = _mapperService.Map<SaveMusicResource, Music>(saveMusicResource);
            await _musicService.UpdateMusic(musicUpdate, music);
            var musicUpdateNew = await _musicService.GetMusicById(id);
            var musicResourceUpdate = _mapperService.Map<Music, SaveMusicResource>(musicUpdateNew);
            return Ok(musicResourceUpdate);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMusic(int id)
        {
            var music = await _musicService.GetMusicById(id);
            if (music == null) return BadRequest("La musique n'existe pas");

            await _musicService.DeleteMusic(music);
            return NoContent();
        }

        [HttpGet("Artist/id")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusicByIdArtist(int id)
        {
            try
            {
                var musics = await _musicService.GetMusicsByArtistId(id);
                if (musics == null) return BadRequest("Pour cet artist il n'ya des musiques");
                var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
                return Ok(musicResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusicCore.Models;
using MyMusicCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IMapper _mapperService;

        public MusicController(IMusicService musicService, IMapper mapperService)
        {
            _musicService = musicService;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusic()
        {
            try
            {
                IEnumerable<Music> musics = await _musicService.GetAllWithArtist();
                //return Ok(musics);
                IEnumerable<MusicResource> musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
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
                Music music = await _musicService.GetMusicById(id);
                if (music == null) return NotFound();
                //mappage
                MusicResource musicResource = _mapperService.Map<Music, MusicResource>(music);
                return Ok(musicResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<MusicResource>> CreateMusic(SaveMusicResource saveMusicResource)
        {
            try
            {
                //GET Current user
                var userId = User.Identity.Name;
                // Validation
                SaveMusicResourceValidator validation = new SaveMusicResourceValidator();
                ValidationResult validationResult = await validation.ValidateAsync(saveMusicResource);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
                // mappage view à la BD
                Music music = _mapperService.Map<SaveMusicResource, Music>(saveMusicResource);
                // Creation de music
                Music newMusic = await _musicService.CreateMusic(music);
                // mappage BD à la view
                MusicResource musicResource = _mapperService.Map<Music, MusicResource>(newMusic);
                return Ok(musicResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MusicResource>> UpdateMusic(int id, SaveMusicResource saveMusicResource)
        {
            try
            {
                // si la music existe depuis le id
                Music musicUpdate = await _musicService.GetMusicById(id);
                if (musicUpdate == null) return BadRequest("la music n'existe pas ");

                /// validation
                SaveMusicResourceValidator validation = new SaveMusicResourceValidator();
                ValidationResult validationResult = await validation.ValidateAsync(saveMusicResource);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                //mappage view to db
                Music music = _mapperService.Map<SaveMusicResource, Music>(saveMusicResource);
                //upadate dans la bd
                await _musicService.UpdateMusic(musicUpdate, music);
                //get the updated music
                Music musicUpdateNew = await _musicService.GetMusicById(id);
                //mappage bd to view
                SaveMusicResource musicResourceUpdate = _mapperService.Map<Music, SaveMusicResource>(musicUpdateNew);
                return Ok(musicResourceUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMusic(int id)
        {
            try
            {
                Music music = await _musicService.GetMusicById(id);
                if (music == null) return BadRequest("La musique n'existe pas");

                await _musicService.DeleteMusic(music);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Artist/{id}")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusicByIdArtist(int id)
        {
            try
            {
                var musics = await _musicService.GetMusicsByArtistId(id);
                if (musics == null) return BadRequest("Pour cet artist il n'ya des musiques");
                IEnumerable<MusicResource> musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
                return Ok(musicResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
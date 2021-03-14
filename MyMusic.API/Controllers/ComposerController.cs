using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MysMusic.Core.Models;
using MysMusic.Core.Services;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComposerController : ControllerBase
    {
        private readonly IComposerService _composerService;
        private readonly IMapper _mapperService;

        public ComposerController(IComposerService composerService, IMapper mapperService)
        {
            _composerService = composerService;
            _mapperService = mapperService;
        }
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ComposerResourse>>> GetAllComposer()
        {
            try
            {
                IEnumerable<Composer> composers = await _composerService.GetAllComposers();
                IEnumerable<ComposerResourse> composerResources = _mapperService.Map<IEnumerable<Composer>, IEnumerable<ComposerResourse>>(composers);
                return Ok(composerResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComposerResourse>> GetComposerById(string id)
        {
            try
            {
                Composer composer = await _composerService.GetComposerById(id);
                if (composer == null) return BadRequest("COMPOSER NOT FOUND !!");
                ComposerResourse composerresource = _mapperService.Map<Composer, ComposerResourse>(composer);
                return Ok(composer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("")]
        public async Task<ActionResult<ComposerResourse>> CreateComposer(SaveComposerResource saveComposerResource)
        {
            try
            {
                //validation
                SaveComposerResourceValidator validation = new SaveComposerResourceValidator();
                ValidationResult validationResult = await validation.ValidateAsync(saveComposerResource);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                // mappage view to bd
                Composer composer = _mapperService.Map<SaveComposerResource, Composer>(saveComposerResource);
                // Create Composer
                Composer composerNew = await _composerService.Create(composer);
                // mappage bd to view
                ComposerResourse composerresource = _mapperService.Map<Composer, ComposerResourse>(composerNew);
                return Ok(composerresource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("")]
        public async Task<ActionResult<ComposerResourse>> UpdateComposer(string id, SaveComposerResource saveComposerResource)
        {
            try
            {
                // si le id existe 
                Composer composerUpdate = await _composerService.GetComposerById(id);
                if (composerUpdate == null) return NotFound();

                //validation
                SaveComposerResourceValidator validation = new SaveComposerResourceValidator();
                ValidationResult validationResult = await validation.ValidateAsync(saveComposerResource);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                // mappage view to db
                Composer composer = _mapperService.Map<SaveComposerResource, Composer>(saveComposerResource);
                _composerService.Update(id, composer);
                // get the new updated composer
                Composer composerNewUpdate = await _composerService.GetComposerById(id);
                //mappage to db to view
                ComposerResourse composerresource = _mapperService.Map<Composer, ComposerResourse>(composerNewUpdate);
                return Ok(composerresource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComposerById(string id)
        {
            try
            {
                Composer composer = await _composerService.GetComposerById(id);
                if (composer == null) return NotFound();
                await _composerService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
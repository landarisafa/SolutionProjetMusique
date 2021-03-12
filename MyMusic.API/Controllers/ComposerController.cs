using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            var composers = await _composerService.GetAllComposers();
            var composerResources= _mapperService.Map<IEnumerable<Composer>, IEnumerable<ComposerResourse>>(composers);
            return Ok(composerResources);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComposerResourse>> GetComposerById(string id)
        {
            try
            {
                var composer = await _composerService.GetComposerById(id);
                if (composer == null) return NotFound();
                var composerresource = _mapperService.Map<Composer, ComposerResourse>(composer);
                return Ok(composer);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("")]
        public async Task<ActionResult<ComposerResourse>> CreateComposer(SaveComposerResource saveComposerResource)
        {

            //validation
            var validation = new SaveComposerResourceValidator();
            var validationResult = await validation.ValidateAsync(saveComposerResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // mappage
            var composer= _mapperService.Map<SaveComposerResource, Composer>(saveComposerResource);
            // Create Composer
            var composerNew=await _composerService.Create(composer);
            var composerresource = _mapperService.Map<Composer, ComposerResourse>(composerNew);
            return Ok(composerresource);

        }
        [HttpPut("")]
        public async Task<ActionResult<ComposerResourse>> UpdateComposer(string id, SaveComposerResource saveComposerResource)
        {

            //validation
            var validation = new SaveComposerResourceValidator();
            var validationResult = await validation.ValidateAsync(saveComposerResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            // si le id existe 
            var composerUpdate = await _composerService.GetComposerById(id);
            if (composerUpdate == null) return NotFound();

            // mappage
            var composer = _mapperService.Map<SaveComposerResource, Composer>(saveComposerResource);
             _composerService.Update(id, composer);
            // mappage
            var composerNewUpdate = await _composerService.GetComposerById(id);
            var composerresource = _mapperService.Map<Composer, ComposerResourse>(composerNewUpdate);
            return Ok(composerresource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComposerById(string id)
        {
            var composer = await _composerService.GetComposerById(id);
            if (composer == null) return NotFound();
            await _composerService.Delete(id);
            return NoContent();

        }


    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMusicCore.Models;
using MyMusicCore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFilesService _fileService;
        private readonly IConfiguration _configuration;

        public FileUploadController(IFilesService musicService, IConfiguration configuration)
        {
            _fileService = musicService;
            _configuration = configuration;

        }

        [HttpPost("")]
        public async Task<Files> OnPostUploadAsync(IFormFile file)
        {
            string filePath = "";
            if (file.Length > 0)
                filePath = Path.Combine(_configuration["pathImagesFolder"], file.FileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            Files fileSaved = await _fileService.CreateFile(filePath);

            return fileSaved;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OnDownloadAsync(string id)
        {
            byte[] fileEncoded = await _fileService.GetFile(id);
            // return File(fileEncoded, "application/png");
            return Ok(fileEncoded);

        }

    }
}

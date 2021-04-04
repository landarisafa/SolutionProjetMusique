using MyMusicCore.Models;
using MyMusicCore.Repository;
using MyMusicCore.Services;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyMusic.Services.Services
{
    public class FilesService : IFilesService
    {
        private readonly IFilesRepository _context;

        public FilesService(IFilesRepository context)
        {
            _context = context;
        }

        public async Task<Files> CreateFile(string pathFile)
        {
            Files file = new Files()
            {
                FilePath = pathFile
            };

            await _context.CreateFile(file);
            return file;
        }

        public async Task<byte[]> GetFile(string id)
        {
            Files file = await _context.GetFile(id);

            string FileDownloadName = Path.GetFileName(file.FilePath);

            if (string.IsNullOrEmpty(file.FilePath))
            {
                throw new Exception($"Missing file {FileDownloadName}");
            }
            if (System.IO.File.Exists(file.FilePath) == false)
            {
                throw new Exception($"Missing file {FileDownloadName}");
            }
            byte[] encodedFile = System.IO.File.ReadAllBytes(file.FilePath);
            return encodedFile;
        }
    }
}

using MyMusicCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicCore.Services
{
    public interface IFilesService
    {
        Task<byte[]> GetFile(string id);
        Task<Files> CreateFile(string pathFile);
    }
}

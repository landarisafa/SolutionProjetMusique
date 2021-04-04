using MyMusicCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicCore.Repository
{
    public interface IFilesRepository
    {
        Task<Files> GetFile(string id);
        Task<Files> CreateFile(Files file);
    }
}

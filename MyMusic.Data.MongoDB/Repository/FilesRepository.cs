using MongoDB.Bson;
using MongoDB.Driver;
using MyMusic.Data.MongoDB.Setting;
using MyMusicCore.Models;
using MyMusicCore.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.MongoDB.Repository
{
    public class FilesRepository : IFilesRepository
    {
        private readonly IDatabaseSettings _context;

        public FilesRepository(IDatabaseSettings context)
        {
            _context = context;

        }

        public async Task<Files> CreateFile(Files file)
        {
            await _context.Files.InsertOneAsync(file);
            return file;
        }

        public async Task<Files> GetFile(string id)
        {
            FilterDefinition<Files> filter = Builders<Files>.Filter.Eq(f=>f.Id, id);

            return await _context
                    .Files
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }

    }
}

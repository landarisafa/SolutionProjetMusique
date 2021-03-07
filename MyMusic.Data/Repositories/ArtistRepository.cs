using Microsoft.EntityFrameworkCore;
using MyMusicCore.Models;
using MyMusicCore.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.Repositories
{
    public class ArtistRepository :Repository<Artist>,IArtistRepository
    {
        private MyMusicDbContext _myMusicDbContext
        {
            get { return _context as MyMusicDbContext; }
        }
        public ArtistRepository(MyMusicDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Artist>> GetAllWithMusicsAsync()
        {
            return await _myMusicDbContext.Artists
                .Include(a => a.Musics)
                .ToListAsync();
        }

        public Task<Artist> GetWithMusicsByIdAsync(int id)
        {
            return _myMusicDbContext.Artists
                .Include(a => a.Musics)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

    }
}

using MyMusicCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicCore.Repository
{
    public interface IMusicRepository :IRepository<Music>
    {
        Task<IEnumerable<Music>> GetAllWithArtistAsync();
        Task<Music> GetWithArtistByIdAsync(int id);
        Task<IEnumerable<Music>> GetAllWithArtistByArtistIdAsync(int artistId);
    }
}

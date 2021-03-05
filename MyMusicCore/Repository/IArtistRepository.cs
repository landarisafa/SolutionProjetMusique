using MyMusicCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicCore.Repository
{
    public interface IArtistRepository :IRepository<Artist>
    {
        Task<IEnumerable<Artist>> GetAllWithMusicsAsync();
        Task<Artist> GetWithMusicsByIdAsync(int id);
    }
}

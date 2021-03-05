using System;
using MyMusicCore.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicCore.Services
{
   public interface IMusicService
    {
        Task<IEnumerable<Music>> GetAllWithArtist();
        Task<Music> GetMusicById(int id);
        Task<IEnumerable<Music>> GetMusicsByArtistId(int artistId);
        Task<Music> CreateMusic(Music newMusic);
        Task UpdateMusic(Music musicToBeUpdated, Music music);
        Task DeleteMusic(Music music);
    }
}

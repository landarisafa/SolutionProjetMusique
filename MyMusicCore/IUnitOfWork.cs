using MyMusicCore.Repository;
using System;
using System.Threading.Tasks;

namespace MyMusicCore
{
    public interface IUnitOfWork :IDisposable
    {
        IArtistRepository Artists { get; }
        IMusicRepository Musics { get; }
        Task<int> CommitAsync();
    }
}

using MyMusicCore.Repositories;
using MyMusicCore.Repository;
using System;
using System.Threading.Tasks;

namespace MyMusicCore
{
    public interface IUnitOfWork :IDisposable
    {
        IArtistRepository Artists { get; }
        IMusicRepository Musics { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}

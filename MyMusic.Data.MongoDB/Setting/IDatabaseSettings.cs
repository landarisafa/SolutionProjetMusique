using MongoDB.Driver;
using MyMusicCore.Models;
using MysMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data.MongoDB.Setting
{
    public interface IDatabaseSettings
    {
        IMongoCollection<Composer> Composers { get; }
        IMongoCollection<Files> Files { get; }
    }
}

using Microsoft.EntityFrameworkCore;
using MyMusic.Data.Configuration;
using MyMusicCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data
{
    public class MyMusicDbContext : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public MyMusicDbContext(DbContextOptions<MyMusicDbContext> options)
       : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new MusicConfiguration());

            builder
                .ApplyConfiguration(new ArtistConfiguration());
        }
    }
}

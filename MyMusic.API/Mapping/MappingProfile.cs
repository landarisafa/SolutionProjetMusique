using AutoMapper;
using MyMusic.API.Resources;
using MyMusicCore.Models;
using MysMusic.Core.Models;

namespace MyMusic.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain(base de donnée )  vers Resource
            CreateMap<Music, MusicResource>();
               // .ForMember(mu=>mu.Name,output=>output.MapFrom(src=>src.Artist.Name);
            CreateMap<Artist, ArtistResource>();
            CreateMap<Music, SaveMusicResource>();
            CreateMap<Artist, SaveArtistResource>();
            CreateMap<Composer, ComposerResourse>();
            CreateMap<Composer, SaveComposerResource>();

            // Resources vers Domain ou la base de données

            CreateMap<MusicResource, Music>();
            CreateMap<ArtistResource, Artist>();
            CreateMap<SaveMusicResource, Music>();
            CreateMap<SaveArtistResource, Artist>();
            CreateMap<ComposerResourse, Composer>();
            CreateMap<SaveComposerResource, Composer>();

        }

    }
}

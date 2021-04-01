using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Resources
{
    //pour vue d'affichage
    public class MusicResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public ArtistResource Artist { get; set; }
    }
}

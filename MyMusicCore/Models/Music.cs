using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusicCore.Models
{
    public class Music
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}

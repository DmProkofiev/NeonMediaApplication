using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.DTO
{
    public class AudioDTO
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public byte[] CoverArt { get; set; }
        public int? TrackNumber { get; set; }
        public string Composer { get; set; }
        public string Lyricist { get; set; }
        public string Comment { get; set; }
        public string Language { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Grouping { get; set; }
        public int? Rating { get; set; }
    }
}

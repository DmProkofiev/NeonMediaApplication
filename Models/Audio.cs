using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models
{

    public class Audio : MediaFile
    {
        public string AudioCodec { get; set; } // Кодек сжатия (MP3, FLAC, AAC и т.д.)
        public int? SampleRate { get; set; } // Частота дискретизации в Гц (44100, 48000 и т.д.)
        public int? TrackNumber { get; set; } // Номер трека в альбоме
        public int? BitRate { get; set; }
        public string Composer { get; set; } // Композитор произведения
        public string Lyricist { get; set; } // Автор текста песни
        public string Comment { get; set; } // Комментарий к файлу
        public string Language { get; set; } // Язык текста песни (например "rus", "eng")
        public DateTime? ReleaseDate { get; set; } // Дата релиза 
        public string Grouping { get; set; } // Группировка для альбомов-сборников, ремиксов
        public int? Rating { get; set; } // Рейтинг трека (от 1 до 5 звёзд)
        public override MediaType Type => MediaType.Audio;
        public override bool CanExportAudio => false;

        public override string GetDescription()
        {
            return $"Аудио: {Title}, {Artist} ({Duration:mm\\:ss})";
        }
    }
}

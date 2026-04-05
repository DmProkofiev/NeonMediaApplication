using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models
{

    public class Audio : MediaFile
    {
        public string AudioCodec { get; set; } // Аудио кодек
        public int? BitRate { get; set; } // Битрейт kbps
        public int? SampleRate { get; set; } // Частота дискретизации 
        public int? TrackNumber { get; set; } // Номер трека в альбоме 
        public override bool CanExportAudio => false; // Запрет на экспорт
        public override MediaType Type => MediaType.Audio;
        public override string GetDescription()
        {
            return $"Аудио: {Title}, {Artist}, ({Duration:mm\\:ss})";
        }
    }
}

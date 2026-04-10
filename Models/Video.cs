using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models
{
    public class Video : MediaFile
    {
        //Обьект для видео, наследник MediaFile
        public int? Width { get; set; } // Ширина в пикселях
        public int? Height { get; set; } // Высота в пикселях
        public string VideoCodec { get; set; } // Видео кодек
        public string AudioCodec { get; set; } // Аудио кодек в видео
        public override bool CanExportAudio => true; // допуск на экспорт
        public override MediaType Type => MediaType.Video;
        public override string GetDescription()
        {
            string resolution = Width.HasValue && Height.HasValue
                ? $"{Width}x{Height}"
                : "разрешение неизвестно";

            return $"Видео: {Title}, {resolution}, ({Duration:mm\\:ss})";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models
{
    public class Video : MediaFile
    {
        public int? Width { get; set; } // Ширина видео (пиксели)
        public int? Height { get; set; } // Высота видео
        public string VideoCodec { get; set; } // Кодек видео (H.264, etc.)
        public string AudioCodec { get; set; } // Кодек аудиодорожки (AAC, etc.)
        public override MediaType Type => MediaType.Video;
        public override bool CanExportAudio => true;
        public override string GetDescription()
        {
            string resolution = Width.HasValue && Height.HasValue
                ? $"{Width}x{Height}"
                : "разрешение неизвестно";
            return $"Видео: {Title}, {resolution} ({Duration:mm\\:ss})";
        }
    }
}

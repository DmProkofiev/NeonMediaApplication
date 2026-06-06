using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models
{
    public abstract class MediaFile
    {
        // Общие поля всех медиафайлов
        public string FilePath { get; set; }// Путь к файлу
        public string FileName => System.IO.Path.GetFileName(FilePath); // Имя файла
        public string Title { get; set; } // Название трека/видео
        public string Artist { get; set; } // Исполнитель (или режиссёр)
        public string Album { get; set; } // Альбом (или сериал)
        public TimeSpan Duration { get; set; } // Длительность
        public string Genre { get; set; } // Жанр (музыка/фильм)
        public int Year { get; set; } // Год выпуска
        public byte[]? CoverArt { get; set; } // Обложка альбома / постер фильма
        public abstract MediaType Type { get; }
        public abstract bool CanExportAudio { get; }
        public abstract string GetDescription();
    }
    public enum MediaType
    {
        Audio,
        Video
    }
}

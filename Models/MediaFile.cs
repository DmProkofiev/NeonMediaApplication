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
        public string FilePath { get; set; } // Путь к файлу на диске
        public string FileName => System.IO.Path.GetFileName(FilePath); // Имя файла без пути
        public string Title { get; set; } //Название трека или видео
        public string Artist { get; set; } // Исполнитель
        public string Album { get; set; } // Альбом (для аудио)
        public TimeSpan Duration { get; set; } // Длительность медиафайла
        public abstract MediaType Type { get; } // Тип медиафайла (аудио или видео)
        public abstract bool CanExportAudio { get; } // Возможность экспорта аудио (для видеофайлов)
        public abstract string GetDescription(); // Метод для получения описания медиафайла
    }
    public enum MediaType
    {
        Audio,
        Video
    }
}

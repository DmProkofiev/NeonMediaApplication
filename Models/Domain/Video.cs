using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models.Domain
{
    /// <summary>
    /// Представляет видеофайл. Наследуется от <see cref="MediaFile"/>.
    /// Содержит дополнительные метаданные, специфичные для видео.
    /// </summary>
    public class Video : MediaFile
    {
        /// <summary>
        /// Описание видеоролика (из тегов контейнера).
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Ключевые слова / теги для поиска.
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// Ширина кадра в пикселях.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Высота кадра в пикселях.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Частота кадров в секунду (fps).
        /// </summary>
        public double? FrameRate { get; set; }

        /// <summary>
        /// Видеокодек (H.264, H.265, VP9, AV1 и др.).
        /// </summary>
        public string? VideoCodec { get; set; }

        /// <summary>
        /// Видеобитрейт в килобитах в секунду.
        /// </summary>
        public int? VideoBitrate { get; set; }

        /// <summary>
        /// Аудиокодек видеодорожки (AAC, MP3, AC3 и др.).
        /// </summary>
        public string? AudioCodec { get; set; }

        /// <summary>
        /// Аудиобитрейт видеодорожки в кбит/с.
        /// </summary>
        public int? AudioBitrate { get; set; }

        /// <summary>
        /// Количество аудиоканалов в видео.
        /// </summary>
        public int? AudioChannels { get; set; }

        /// <summary>
        /// Частота дискретизации аудио в видео (Гц).
        /// </summary>
        public int? AudioSampleRate { get; set; }

        /// <summary>
        /// Список путей к внешним файлам субтитров или встроенные дорожки.
        /// </summary>
        public List<string> Subtitles { get; set; } = new();

        /// <summary>
        /// Соотношение сторон кадра (16:9, 4:3, 21:9 и др.).
        /// </summary>
        public string? AspectRatio { get; set; }

        /// <summary>
        /// Тип медиафайла. Всегда возвращает <see cref="MediaType.Video"/>.
        /// </summary>
        public override MediaType MediaType => MediaType.Video;

        /// <summary>
        /// Допустимость экспорта аудио. Для видеофайлов возвращает <see langword="true"/>.
        /// </summary>
        public override bool CanExportAudio => true;

        /// <summary>
        /// Возвращает текстовое описание видеофайла.
        /// </summary>
        /// <returns>Строка с описанием видеофайла.</returns>
        public override string GetDescription()
        {
            string resolution = Width.HasValue && Height.HasValue
                ? $"{Width}x{Height}"
                : "разрешение неизвестно";
            return $"Видео: {Title}, {resolution} ({Duration:mm\\:ss})";
        }
    }
}

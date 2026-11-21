using System;
using System.Collections.Generic;

namespace NeonMediaApplication.Models.DTO
{
    /// <summary>
    /// Объект передачи данных для редактируемых пользователем полей видеофайла.
    /// Используется для передачи данных между представлением (View) и слоем бизнес-логики.
    /// </summary>
    public class VideoDTO
    {
        /// <summary>
        /// Название видеоролика.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Описание видеоролика.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Ключевые слова / теги для поиска.
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// Жанр видео.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска видео.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Пользовательский рейтинг (0–5).
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Данные обложки / постера видео в байтах (JPEG/PNG).
        /// </summary>
        public byte[]? CoverArt { get; set; }

        /// <summary>
        /// Список путей к файлам субтитров.
        /// </summary>
        public List<string> Subtitles { get; set; } = new();
    }
}

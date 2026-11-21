using System;

namespace NeonMediaApplication.Models.DTO
{
    /// <summary>
    /// Объект передачи данных для редактируемых пользователем полей аудиофайла.
    /// Аналог окна «Детали» в iTunes. Используется для передачи данных между представлением (View)
    /// и слоем бизнес-логики.
    /// </summary>
    public class AudioDTO
    {
        /// <summary>
        /// Название трека или песни.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Основной исполнитель / артист.
        /// </summary>
        public string Artist { get; set; } = string.Empty;

        /// <summary>
        /// Название альбома.
        /// </summary>
        public string Album { get; set; } = string.Empty;

        /// <summary>
        /// Жанр медиафайла.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска трека или альбома.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Пользовательский рейтинг (0–5 звёзд или 0–100).
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Комментарии пользователя к медиафайлу.
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Номер дорожки в альбоме.
        /// </summary>
        public int? TrackNumber { get; set; }

        /// <summary>
        /// Общее количество дорожек в альбоме.
        /// </summary>
        public int? TrackTotal { get; set; }

        /// <summary>
        /// Номер диска (для много-дисковых альбомов).
        /// </summary>
        public int? DiscNumber { get; set; }

        /// <summary>
        /// Общее количество дисков в альбоме.
        /// </summary>
        public int? DiscTotal { get; set; }

        /// <summary>
        /// Композитор произведения.
        /// </summary>
        public string Composer { get; set; } = string.Empty;

        /// <summary>
        /// Исполнитель, если отличается от основного артиста.
        /// </summary>
        public string Performer { get; set; } = string.Empty;

        /// <summary>
        /// Группировка треков.
        /// </summary>
        public string Grouping { get; set; } = string.Empty;

        /// <summary>
        /// Темп произведения (ударов в минуту, BPM).
        /// </summary>
        public int? Bpm { get; set; }

        /// <summary>
        /// Признак сборника (треки разных исполнителей в одном альбоме).
        /// </summary>
        public bool IsCompilation { get; set; }

        /// <summary>
        /// Данные обложки альбома в байтах (JPEG/PNG).
        /// </summary>
        public byte[]? CoverArt { get; set; }

        /// <summary>
        /// Текст песни.
        /// </summary>
        public string Lyrics { get; set; } = string.Empty;
    }
}

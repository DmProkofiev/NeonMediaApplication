using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models.Domain
{
    /// <summary>
    /// Абстрактный базовый класс для всех медиафайлов.
    /// Содержит общие метаданные, характерные как для аудио, так и для видео.
    /// </summary>
    public abstract class MediaFile : INotifyPropertyChanged
    {
        /// <summary>
        /// Уникальный идентификатор записи в библиотеке приложения. Генерируется при добавлении файла.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Полный абсолютный путь к медиафайлу на диске.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Имя файла с расширением. Вычисляется на основе <see cref="FilePath"/>.
        /// </summary>
        public string FileName => System.IO.Path.GetFileName(FilePath);

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Продолжительность медиафайла (трека или видео).
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Тип медиафайла: Audio или Video.
        /// </summary>
        public abstract MediaType MediaType { get; }

        /// <summary>
        /// Название трека, песни или видеоролика. Отображается в списке библиотеки.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Основной исполнитель / артист. Для видео — создатель или канал.
        /// </summary>
        public string Artist { get; set; } = string.Empty;

        /// <summary>
        /// Название альбома, к которому относится трек. Для видео может быть пустым.
        /// </summary>
        public string Album { get; set; } = string.Empty;

        /// <summary>
        /// Жанр медиафайла.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска трека, альбома или видео.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Пользовательский рейтинг (например, 0–5 звёзд или 0–100).
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Комментарии пользователя к медиафайлу.
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Количество воспроизведений. Увеличивается при каждом проигрывании.
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// Данные обложки альбома / постера видео в байтах (JPEG/PNG).
        /// </summary>
        public byte[]? CoverArt { get; set; }

        /// <summary>
        /// Текст песни (для аудио) или описание (для видео).
        /// </summary>
        public string Lyrics { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время добавления файла в библиотеку.
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Дата и время последнего изменения метаданных или файла.
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Контрольная сумма файла (MD5/SHA-256) для идентификации дублей и проверки целостности.
        /// </summary>
        public string? Checksum { get; set; }

        /// <summary>
        /// Поле для сортировки по названию (используется вместо Title при сортировке).
        /// </summary>
        public string SortTitle { get; set; } = string.Empty;

        /// <summary>
        /// Поле для сортировки по артисту.
        /// </summary>
        public string SortArtist { get; set; } = string.Empty;

        /// <summary>
        /// Поле для сортировки по альбому.
        /// </summary>
        public string SortAlbum { get; set; } = string.Empty;

        /// <summary>
        /// Допустимость экспорта аудио из файла.
        /// </summary>
        public abstract bool CanExportAudio { get; }

        /// <summary>
        /// Возвращает текстовое описание медиафайла.
        /// </summary>
        /// <returns>Строка с описанием медиафайла.</returns>
        public abstract string GetDescription();

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Уведомляет подписчики об изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя изменённого свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Тип медиафайла.
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// Аудиофайл.
        /// </summary>
        Audio,

        /// <summary>
        /// Видеофайл.
        /// </summary>
        Video
    }
}

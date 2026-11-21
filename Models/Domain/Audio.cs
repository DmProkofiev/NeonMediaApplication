using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models.Domain
{
    /// <summary>
    /// Представляет аудиофайл. Наследуется от <see cref="MediaFile"/>.
    /// Содержит дополнительные метаданные, специфичные для аудиопроизведений.
    /// </summary>
    public class Audio : MediaFile
    {
        /// <summary>
        /// Номер дорожки в альбоме (трек №).
        /// </summary>
        public int? TrackNumber { get; set; }

        /// <summary>
        /// Общее количество дорожек в альбоме.
        /// </summary>
        public int? TrackTotal { get; set; }

        /// <summary>
        /// Номер диска (для много-дисковых альбомов, сборников).
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
        /// Дирижёр (для классической музыки).
        /// </summary>
        public string Conductor { get; set; } = string.Empty;

        /// <summary>
        /// Исполнитель, если отличается от основного артиста.
        /// </summary>
        public string Performer { get; set; } = string.Empty;

        /// <summary>
        /// Группировка треков (например, части произведений в классике).
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
        /// Аудиобитрейт в килобитах в секунду.
        /// </summary>
        public int? Bitrate { get; set; }

        /// <summary>
        /// Частота дискретизации в Герцах.
        /// </summary>
        public int? SampleRate { get; set; }

        /// <summary>
        /// Количество аудиоканалов (1 — моно, 2 — стерео и т.д.).
        /// </summary>
        public int? Channels { get; set; }

        /// <summary>
        /// Аудиокодек (MP3, AAC, FLAC, Opus, WMA и др.).
        /// </summary>
        public string? Codec { get; set; }

        /// <summary>
        /// Инструмент/кодировщик, использованный для создания файла.
        /// </summary>
        public string? EncodingTool { get; set; }

        /// <summary>
        /// Издатель / лейбл.
        /// </summary>
        public string? Publisher { get; set; }

        /// <summary>
        /// Информация об авторских правах.
        /// </summary>
        public string? Copyright { get; set; }

        /// <summary>
        /// Тип медиафайла. Всегда возвращает <see cref="MediaType.Audio"/>.
        /// </summary>
        public override MediaType MediaType => MediaType.Audio;

        /// <summary>
        /// Допустимость экспорта аудио. Для аудиофайлов возвращает <see langword="false"/>,
        /// так как файл уже является аудио.
        /// </summary>
        public override bool CanExportAudio => false;

        /// <summary>
        /// Возвращает текстовое описание аудиофайла.
        /// </summary>
        /// <returns>Строка с описанием аудиофайла.</returns>
        public override string GetDescription()
        {
            return $"Аудио: {Title}, {Artist} ({Duration:mm\\:ss})";
        }
    }
}

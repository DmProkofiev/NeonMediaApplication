using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Models.Domain
{
    /// <summary>
    /// Плейлист, содержащий коллекцию медиафайлов.
    /// Может быть сохранён в формате JSON или XML для последующего импорта/экспорта.
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Уникальный идентификатор плейлиста.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название плейлиста.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание плейлиста.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Коллекция медиафайлов, входящих в плейлист.
        /// </summary>
        public ObservableCollection<MediaFile> Items { get; set; } = new();

        /// <summary>
        /// Дата и время создания плейлиста.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата и время последнего изменения плейлиста.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}

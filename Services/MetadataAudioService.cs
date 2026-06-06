using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace NeonMediaApplication.Services
{
    public class MetadataAudioService : IMetadataAudioService
    {
        public async Task<Audio> ReadMetadataAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                if (!global::System.IO.File.Exists(filePath))
                    throw new FileNotFoundException($"Файл не найден: {filePath}");

                using (var file = TagLib.File.Create(filePath))
                {
                    var tag = file.Tag;
                    var props = file.Properties;

                    return new Audio
                    {
                        FilePath = filePath,
                        Title = tag.Title ?? Path.GetFileNameWithoutExtension(filePath),
                        Artist = tag.FirstPerformer ?? (tag.AlbumArtists?.FirstOrDefault() ?? "Unknown Artist"),
                        Album = tag.Album ?? "Unknown Album",
                        Duration = props.Duration,
                        Genre = tag.Genres?.FirstOrDefault() ?? "",
                        Year = (int)tag.Year,
                        TrackNumber = (int)tag.Track,
                        AudioCodec = GetAudioCodecDescription(props, filePath),
                        BitRate = (int?)props.AudioBitrate,
                        SampleRate = props.AudioSampleRate,
                        CoverArt = GetCoverArtBytes(file)
                    };
                }
            });
        }

        public async Task SaveMetadataAsync(Audio audio)
        {
            if (audio == null) throw new ArgumentNullException(nameof(audio));
            if (string.IsNullOrEmpty(audio.FilePath)) throw new ArgumentException("FilePath не может быть пустым");

            await Task.Run(() =>
            {
                using (var file = TagLib.File.Create(audio.FilePath))
                {
                    var tag = file.Tag;
                    tag.Title = audio.Title;
                    tag.Performers = string.IsNullOrEmpty(audio.Artist) ? null : new[] { audio.Artist };
                    tag.Album = audio.Album;
                    tag.Genres = string.IsNullOrEmpty(audio.Genre) ? null : new[] { audio.Genre };

                    if (audio.Year > 0)
                        tag.Year = (uint)audio.Year;

                    if (audio.TrackNumber.HasValue && audio.TrackNumber.Value > 0)
                        tag.Track = (uint)audio.TrackNumber.Value;

                    if (!string.IsNullOrEmpty(audio.Composer))
                        tag.Composers = new[] { audio.Composer };

                    if (!string.IsNullOrEmpty(audio.Comment))
                        tag.Comment = audio.Comment;

                    if (!string.IsNullOrEmpty(audio.Grouping))
                        tag.Grouping = audio.Grouping;


                    file.Save();
                }
            });
        }

        public async Task<byte[]> ReadCoverArtAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                if (!global::System.IO.File.Exists(filePath))
                    throw new FileNotFoundException($"Файл не найден: {filePath}");

                using (var file = TagLib.File.Create(filePath))
                {
                    return GetCoverArtBytes(file);
                }
            });
        }

        public async Task SaveCoverArtAsync(string filePath, byte[] coverArtBytes)
        {
            if (coverArtBytes == null || coverArtBytes.Length == 0)
                throw new ArgumentException("Обложка не может быть пустой");
            if (!global::System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            await Task.Run(() =>
            {
                using (var file = TagLib.File.Create(filePath))
                {
                    var picture = new Picture(new ByteVector(coverArtBytes))
                    {
                        Type = PictureType.FrontCover,
                        MimeType = GetMimeType(coverArtBytes)
                    };
                    file.Tag.Pictures = new IPicture[] { picture };
                    file.Save();
                }
            });
        }

        private static byte[] GetCoverArtBytes(TagLib.File file)
        {
            if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                return file.Tag.Pictures[0].Data.Data;
            return null;
        }

        private static string GetMimeType(byte[] bytes)
        {
            if (bytes.Length > 3 && bytes[0] == 0xFF && bytes[1] == 0xD8)
                return "image/jpeg";
            if (bytes.Length > 3 && bytes[0] == 0x89 && bytes[1] == 0x50)
                return "image/png";
            return "image/jpeg";
        }

        private static string GetAudioCodecDescription(Properties props, string filePath)
        {
            var audioCodec = props.Codecs.OfType<IAudioCodec>().FirstOrDefault();
            if (audioCodec != null && !string.IsNullOrEmpty(audioCodec.Description))
                return audioCodec.Description;

            var ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".mp3" => "MP3",
                ".flac" => "FLAC",
                ".m4a" => "AAC",
                ".wma" => "WMA",
                ".ogg" => "Vorbis",
                _ => "Unknown"
            };
        }
    }
}

using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp;
using TagLib;
using System.Windows;

namespace NeonMediaApplication.Services
{
    //Класс: Сервис парсинга файладля передачи в МедиаДвижок
    //Задача: Парсинг файла, создание обьекта модели(Audio/Video), передача в движок
    public class FileService : IFileService
    {
        public async Task<MediaFile> ParseMediaAsync(string filePath)
        {
            return await Task.Run(() => GetFile(filePath));
        }

        public MediaFile GetFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            using (var item = TagLib.File.Create(filePath))
            {
                if (extension == ".mp3" || extension == ".flac" || extension == ".wav" || extension == ".m4a")
                {
                    return new Audio
                    {
                        FilePath = filePath,
                        Title = GetTitle(item, filePath),
                        Artist = GetArtist(item),
                        Album = item.Tag.Album ?? "Неизвестный Альбом",
                        Duration = item.Properties.Duration,
                        AudioCodec = GetAudioCodec(item),
                        BitRate = (int?)item.Properties.AudioBitrate,
                        SampleRate = item.Properties.AudioSampleRate,
                        TrackNumber = (int?)item.Tag.Track
                    };
                }
                else if (extension == ".mp4" || extension == ".mkv" || extension == ".avi" || extension == ".wmv")
                {
                    return new Video
                    {
                        FilePath = filePath,
                        Title = GetTitle(item, filePath),
                        Artist = GetArtist(item),
                        Album = item.Tag.Album ?? "Неизвестный Альбом",
                        Duration = item.Properties.Duration,
                        Width = item.Properties.VideoWidth,
                        Height = item.Properties.VideoHeight,
                        VideoCodec = GetVideoCodec(item),
                        AudioCodec = GetAudioCodec(item)
                    };
                }
                else
                {
                    throw new NotSupportedException($"Формат {extension} не поддерживается");
                }
            }
        }

        private string GetTitle(TagLib.File tagFile, string filePath)
        {
            if (!string.IsNullOrEmpty(tagFile.Tag.Title))
                return tagFile.Tag.Title;

            return Path.GetFileNameWithoutExtension(filePath);
        }

        private string GetArtist(TagLib.File tagFile)
        {
            if (tagFile.Tag.FirstPerformer != null)
                return tagFile.Tag.FirstPerformer;

            if (tagFile.Tag.AlbumArtists != null && tagFile.Tag.AlbumArtists.Length > 0)
                return tagFile.Tag.AlbumArtists[0];

            return "Неизвестный Артист";
        }

        private string GetAudioCodec(TagLib.File tagFile)
        {
            var audioCodecs = tagFile.Properties.Codecs.OfType<TagLib.IAudioCodec>().ToList();
            if (audioCodecs.Any())
            {
                var firstAudioStream = audioCodecs.First();
                return firstAudioStream.Description ?? "Неизвестный" ;
            }
            return "Неизвестный";
        }

        private string GetVideoCodec(TagLib.File tagFile)
        {
            var videoCodecs = tagFile.Properties.Codecs.OfType<TagLib.IVideoCodec>().ToList();
            if (videoCodecs.Any())
            {
                var firstVideoStream = videoCodecs.First();
                return firstVideoStream.Description ?? "Неизвестный";
            }
            return "Неизвестный";
        }
    }
}

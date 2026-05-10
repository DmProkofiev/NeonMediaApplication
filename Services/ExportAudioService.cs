using NeonMediaApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace NeonMediaApplication.Services
{
    // Сервис для Экспорта аудио из видео
    public class ExportAudioService : IExportAudioService
    {
        private readonly string _ffmpegPath;

        public ExportAudioService(string ffmpegPath = "ffmpeg.exe")
        {
            _ffmpegPath = ffmpegPath;

            if (!File.Exists(_ffmpegPath))
                throw new FileNotFoundException($"FFmpeg не найден: {_ffmpegPath}");
        }

        public async Task ExportAudioAsync(string videoFilePath, string outputFilePath)
        {
            if (!File.Exists(videoFilePath))
                throw new FileNotFoundException($"Видеофайл не найден: {videoFilePath}");

            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(outputDir))
                throw new DirectoryNotFoundException($"Папка назначения не существует: {outputDir}");

            await Task.Run(() => ConvertVideoToAudio(videoFilePath, outputFilePath));
        }

        private void ConvertVideoToAudio(string videoFilePath, string outputFilePath)
        {
            string extension = Path.GetExtension(outputFilePath).ToLower();
            string audioParams = GetAudioParameters(extension);

            if (string.IsNullOrEmpty(audioParams))
                throw new NotSupportedException($"Формат {extension} не поддерживается");

            string arguments = $"-i \"{videoFilePath}\" -vn {audioParams} -y \"{outputFilePath}\"";

            var startInfo = new ProcessStartInfo(_ffmpegPath, arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();

                string error = process.StandardError.ReadToEnd();
                string output = process.StandardOutput.ReadToEnd();
                string message = string.IsNullOrEmpty(error) ? output : error;

                if (process.ExitCode != 0)
                    throw new Exception($"FFmpeg ошибка (код {process.ExitCode}): {message}");
            }
        }

        private string GetAudioParameters(string extension)
        {
            switch (extension)
            {
                case ".mp3": return "-acodec libmp3lame -ab 192k";
                case ".aac":
                case ".m4a": return "-acodec aac -b:a 128k";
                case ".flac": return "-acodec flac";
                case ".ogg":
                case ".oga": return "-acodec libvorbis -q:a 5";
                case ".wma": return "-acodec wmav2 -ab 128k";
                case ".ac3": return "-acodec ac3 -ab 192k";
                case ".opus": return "-acodec libopus -ab 128k";
                case ".alac": return "-acodec alac";
                default: return null;
            }
        }
    }
}

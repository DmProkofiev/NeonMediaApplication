using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Interfaces
{
    //Экспорт аудио из видео
    public interface IExportAudioService
    {
        Task ExportAudioAsync(string videoFilePath, string outputFilePath);
    }
}

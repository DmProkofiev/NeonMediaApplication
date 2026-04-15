using NeonMediaApplication.Models;
using NeonMediaApplication.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Interfaces
{
    public interface IMediaEngine
    {
        Task<bool> ReadMediaAsync();
        LibVLCSharp.Shared.MediaPlayer MediaPlayer { get; }
        Task PlayAsync();
        Task StopAsync();
        Task PauseAsync();
        Task SeekAsync(TimeSpan duration);
        Task SetVolumeAsync(int volume);
        void Load(string filePath);
    }
}

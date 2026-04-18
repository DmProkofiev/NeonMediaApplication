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
        event Action<TimeSpan> PositionChanged;
        event Action MediaEnded;
        event Action<bool> PlayingStateChanged;
        LibVLCSharp.Shared.MediaPlayer MediaPlayer { get; }
        void Load(string filePath);
        Task<bool> ReadMediaAsync();
        Task PlayAsync();
        Task StopAsync();
        Task PauseAsync();
        Task SeekAsync(TimeSpan position);
        Task SetVolumeAsync(int volume);
        TimeSpan Duration { get; }
        TimeSpan CurrentPosition { get; }
        bool IsPlaying { get; }
    }
}

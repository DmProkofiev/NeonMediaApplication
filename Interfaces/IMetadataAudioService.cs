using NeonMediaApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonMediaApplication.Interfaces
{
    public interface IMetadataAudioService
    {
        Task<Audio> ReadMetadataAsync(string filePath);
        Task SaveMetadataAsync(Audio audio);
        Task<byte[]> ReadCoverArtAsync(string filePath);
        Task SaveCoverArtAsync(string filePath, byte[] coverArtBytes);
    }
}

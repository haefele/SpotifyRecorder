using System.Collections;
using System.Collections.Generic;
using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface IAudioOutputDeviceService
    {
        IEnumerable<AudioOutputDevice> GetOutputDevices();
    }
}
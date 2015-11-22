using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioOutputDeviceService : IAudioOutputDeviceService
    {
        public IEnumerable<AudioOutputDevice> GetOutputDevices()
        {
            return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
                .Select(f => new AudioOutputDevice
                {
                    Name = f.FriendlyName
                });
        }
    }
}
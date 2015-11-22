using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface IAudioRecordingService
    {
        IAudioRecorder StartRecording(AudioOutputDevice device, Song song);
    }
}
using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface IAudioRecordingService
    {
        IAudioRecorder StartRecording(Song song);
    }
}
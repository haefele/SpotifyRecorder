using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface IAudioRecorder
    {
        Song Song { get; }

        RecordedSong StopRecording();
    }
}
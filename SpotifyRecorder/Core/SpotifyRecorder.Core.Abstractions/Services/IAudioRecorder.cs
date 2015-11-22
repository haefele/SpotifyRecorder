using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface IAudioRecorder
    {
        AudioOutputDevice Device { get; }
        Song Song { get; }

        RecordedSong StopRecording();
    }
}
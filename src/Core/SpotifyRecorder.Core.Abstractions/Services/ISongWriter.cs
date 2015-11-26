using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface ISongWriter
    {
        bool WriteSong(RecordedSong song);
    }
}
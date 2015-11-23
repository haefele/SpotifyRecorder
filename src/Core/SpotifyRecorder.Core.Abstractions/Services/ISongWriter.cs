using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface ISongWriter
    {
        void WriteSong(RecordedSong song);
    }
}
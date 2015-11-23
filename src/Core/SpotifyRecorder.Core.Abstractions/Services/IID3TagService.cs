using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface IID3TagService
    {
        ID3Tags GetTags(RecordedSong song);
        void UpdateTags(ID3Tags tags, RecordedSong song);
    }
}
namespace SpotifyRecorder.Core.Abstractions.Entities
{
    public class RecordedSong
    {
        public Song Song { get; set; }
        public byte[] Data { get; set; } 
    }
}
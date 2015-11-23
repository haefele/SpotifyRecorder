using System.IO;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class SongWriter : ISongWriter
    {
        private readonly ISettings _settings;

        public SongWriter(ISettings settings)
        {
            this._settings = settings;
        }

        public void WriteSong(RecordedSong song)
        {
            string filePath = Path.Combine(this._settings.OutputDirectory, $"{song.Song.Artist} - {song.Song.Title}.mp3");
            
            if (this._settings.SkipExistingSongs && File.Exists(filePath))
                return;
            
            if (Directory.Exists(this._settings.OutputDirectory) == false)
                Directory.CreateDirectory(this._settings.OutputDirectory);

            File.WriteAllBytes(filePath, song.Data);
        }
    }
}
using System.IO;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Extensions;
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

        public bool WriteSong(RecordedSong song)
        {
            string filePath = Path.Combine(this._settings.OutputDirectory, $"{song.Song.Artist.ToValidFileName()} - {song.Song.Title.ToValidFileName()}.mp3");
            
            if (this._settings.SkipExistingSongs && File.Exists(filePath))
                return false;
            
            if (Directory.Exists(this._settings.OutputDirectory) == false)
                Directory.CreateDirectory(this._settings.OutputDirectory);

            File.WriteAllBytes(filePath, song.Data);

            return true;
        }
    }
}
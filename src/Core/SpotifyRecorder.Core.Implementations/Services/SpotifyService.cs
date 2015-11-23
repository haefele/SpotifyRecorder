using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using SpotifyAPI.Local;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly ISettings _settings;
        private readonly SpotifyLocalAPI _localApi;

        public SpotifyService(ISettings settings)
        {
            this._settings = settings;

            if (SpotifyLocalAPI.IsSpotifyRunning() == false)
                SpotifyLocalAPI.RunSpotify();

            this._localApi = new SpotifyLocalAPI();
            this._localApi.Connect();
        }

        public IObservable<Song> GetSong()
        {
            return Observable.Interval(TimeSpan.FromMilliseconds(this._settings.MillisecondsToCheckForTrackChanges))
                .Select(_ =>
                {
                    var songProcessTitle = Process
                        .GetProcessesByName(Constants.SpotifyApplicationName)
                        .Select(f => f.MainWindowTitle)
                        .FirstOrDefault(f => f.Length > Constants.SpotifyApplicationName.Length);

                    return songProcessTitle;
                })
                .DistinctUntilChanged()
                .Select(f =>
                {
                    if (f == null)
                        return null;

                    var currentStatus = this._localApi.GetStatus();
                    return new Song
                    {
                        Title = currentStatus.Track.TrackResource.Name,
                        Artist = currentStatus.Track.ArtistResource.Name,
                        Album = currentStatus.Track.AlbumResource.Name
                    };
                });
        }
    }
}
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class SpotifyService : ISpotifyService
    {
        public IObservable<Song> GetSong()
        {
            return Observable.Interval(TimeSpan.FromMilliseconds(10))
                .Select(_ =>
                {
                    var songProcessTitle = Process
                        .GetProcessesByName(Constants.SpotifyApplicationName)
                        .Select(f => f.MainWindowTitle)
                        .FirstOrDefault(f => f.Length > Constants.SpotifyApplicationName.Length);

                    if (songProcessTitle == null)
                        return null;

                    var parts = songProcessTitle.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
                    
                    return new Song
                    {
                        Artist = parts[0].Trim(),
                        Title = parts[1].Trim()
                    };
                })
                .DistinctUntilChanged();
        }
    }
}
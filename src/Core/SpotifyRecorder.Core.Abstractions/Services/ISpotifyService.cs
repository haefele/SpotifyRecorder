using System;
using SpotifyRecorder.Core.Abstractions.Entities;

namespace SpotifyRecorder.Core.Abstractions.Services
{
    public interface ISpotifyService
    {
        IObservable<Song> GetSong();
    }
}
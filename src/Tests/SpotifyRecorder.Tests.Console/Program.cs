using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Extensions;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;
using SpotifyRecorder.Core.Implementations.Services;
using SpotifyRecorder.Core.Implementations.Settings;

namespace SpotifyRecorder.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new InMemorySettings
            {
                BitRate = BitRate.Extreme,
                MillisecondsToCheckForTrackChanges = 10,
                RecorderDeviceName = "Realtek High Definition Audio",
                OutputDirectory = Path.Combine(".", "Music"),
                SkipExistingSongs = true
            };
            
            var recordingService = new AudioRecordingService(settings);
            var spotifyService = new SpotifyService(settings);
            var writer = new SongWriter(settings);

            IAudioRecorder currentRecorder = null;

            spotifyService.GetSong().Subscribe(song =>
            {
                if (currentRecorder != null)
                {
                    System.Console.WriteLine($"Song {currentRecorder.Song.Title} finished.");
                    StopRecording(currentRecorder, writer);
                }
                
                if (song != null)
                {
                    System.Console.WriteLine($"Recording song {song.Title}");
                    currentRecorder = recordingService.StartRecording(song);
                }
                else
                {
                    System.Console.WriteLine("Currently no song is playing.");
                    currentRecorder = null;
                }
            });
            
            System.Console.ReadLine();
        }

        private static void StopRecording(IAudioRecorder recorder, ISongWriter writer)
        {
            Task.Run(() =>
            {
                var recorded = recorder.StopRecording();

                if (recorded == null)
                    return;

                ID3TagService service = new ID3TagService();

                var tags = service.GetTags(recorded);
                tags.Artists = new[] { recorded.Song.Artist };
                tags.Title = recorded.Song.Title;

                service.UpdateTags(tags, recorded);
                
                writer.WriteSong(recorded);
            });
        }
    }
}

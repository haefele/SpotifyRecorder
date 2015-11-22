using System;
using System.IO;
using System.Linq;
using NAudio.Wave;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Extensions;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Implementations.Services;

namespace SpotifyRecorder.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var recordingService = new AudioRecordingService();
            var spotifyService = new SpotifyService();

            IAudioRecorder currentRecorder = null;
            AudioOutputDevice stereoMix = new AudioOutputDeviceService().GetOutputDevices().FirstOrDefault(f => f.Name.Contains("Stereo"));

            spotifyService.GetSong().Subscribe(song =>
            {
                if (currentRecorder != null)
                {
                    System.Console.WriteLine($"Song {currentRecorder.Song.Title} finished.");
                    var recorded = currentRecorder.StopRecording();

                    ID3TagService service = new ID3TagService();
                    var tags = service.GetTags(recorded);
                    tags.Artists = new[] {recorded.Song.Artist};
                    tags.Title = recorded.Song.Title;

                    service.UpdateTags(tags, recorded);

                    string fileName = $"{recorded.Song.Artist} - {recorded.Song.Title}.mp3".ToValidFileName();
                    File.WriteAllBytes(Path.Combine(".", fileName), recorded.Data);
                }

                if (song != null)
                {
                    System.Console.WriteLine($"Recording song {song.Title}");
                    currentRecorder = recordingService.StartRecording(stereoMix, song);
                }
                else
                {
                    System.Console.WriteLine("Currently no song is playing.");
                    currentRecorder = null;
                }
            });
            
            System.Console.ReadLine();
        }
    }
}

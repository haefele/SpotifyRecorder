using System;
using System.IO;
using System.Linq;
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

            spotifyService.GetSong().Subscribe(song =>
            {
                if (currentRecorder != null)
                {
                    System.Console.WriteLine($"Song {currentRecorder.Song.Title} finished.");
                    var recorded = currentRecorder.StopRecording();
                    File.WriteAllBytes($".\\{recorded.Song.Artist} - {recorded.Song.Title}.wav", recorded.Data);
                }

                if (song != null)
                {
                    System.Console.WriteLine($"Recording song {song.Title}");
                    currentRecorder = recordingService.StartRecording(song);
                }
                else
                {
                    System.Console.WriteLine("Currently no song is playing.");
                }
            });
            
            System.Console.ReadLine();
        }
    }
}

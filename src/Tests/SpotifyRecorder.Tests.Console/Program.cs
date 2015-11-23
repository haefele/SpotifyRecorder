using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    StopRecording(currentRecorder);
                }

                
                if (song != null)
                {
                    var expectedFilePath = GetFilePath(song);

                    if (File.Exists(expectedFilePath))
                    {
                        System.Console.WriteLine($"Currently playing {song.Title}"); 
                        return;
                    }
                    else
                    {
                        
                        System.Console.WriteLine($"Recording song {song.Title}");
                        currentRecorder = recordingService.StartRecording(stereoMix, song);
                    }
                }
                else
                {
                    System.Console.WriteLine("Currently no song is playing.");
                    currentRecorder = null;
                }
            });
            
            System.Console.ReadLine();
        }

        private static string GetFilePath(Song song)
        {
            var fileName = $"{song?.Artist} - {song?.Title}.mp3".ToValidFileName();
            return Path.Combine(".", "Music", fileName);
        }

        private static void StopRecording(IAudioRecorder recorder)
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

                var filePath = GetFilePath(recorded.Song);

                if (File.Exists(filePath))
                    return;

                if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                File.WriteAllBytes(filePath, recorded.Data);
            });
        }
    }
}

using System;
using System.Threading.Tasks;
using ColoredConsole;
using SpotifyRecorder.Core.Abstractions.Services;
using static ColoredConsole.ColorConsole;

namespace SpotifyRecorder.App.Console
{
    public class App
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IAudioRecordingService _recordingService;
        private readonly ISongWriter _songWriter;
        private readonly IID3TagService _id3TagService;

        public App(ISpotifyService spotifyService, IAudioRecordingService recordingService, ISongWriter songWriter, IID3TagService id3TagService)
        {
            this._spotifyService = spotifyService;
            this._recordingService = recordingService;
            this._songWriter = songWriter;
            this._id3TagService = id3TagService;
        }

        public void Run()
        {
            IAudioRecorder currentRecorder = null;

            this._spotifyService.GetSong().Subscribe(song =>
            {
                if (currentRecorder != null)
                {
                    WriteLine(new [] { "Song ", currentRecorder.Song.Title.DarkGreen(), " is finish." }.Coalesce(ConsoleColor.White, ConsoleColor.Green));
                    this.StopRecording(currentRecorder, this._songWriter, this._id3TagService);
                }

                if (song != null)
                {
                    WriteLine(new [] { "Recording song ", song.Title.Blue(), "." }.Coalesce(ConsoleColor.White, ConsoleColor.Cyan));
                    currentRecorder = this._recordingService.StartRecording(song);
                }
                else
                {
                    WriteLine(new [] { "Currently ", "no song".Red(), " is playing." }.Coalesce(ConsoleColor.White, ConsoleColor.DarkGray));
                    currentRecorder = null;
                }
            });

            System.Console.ReadLine();
        }

        private void StopRecording(IAudioRecorder recorder, ISongWriter writer, IID3TagService id3TagService)
        {
            Task.Run(() =>
            {
                var recorded = recorder.StopRecording();

                if (recorded == null)
                    return;
                
                var tags = id3TagService.GetTags(recorded);
                tags.Artists = new[] { recorded.Song.Artist };
                tags.Title = recorded.Song.Title;

                id3TagService.UpdateTags(tags, recorded);

                writer.WriteSong(recorded);
            });
        }
    }
}
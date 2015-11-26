using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoredConsole;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using TagLib.Riff;
using static ColoredConsole.ColorConsole;

namespace SpotifyRecorder.App.Console
{
    public class App
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IAudioRecordingService _recordingService;
        private readonly ISongWriter _songWriter;
        private readonly IID3TagService _id3TagService;

        private readonly IList<Song> _recordedSongs; 
        private IAudioRecorder _currentRecorder;

        public App(ISpotifyService spotifyService, IAudioRecordingService recordingService, ISongWriter songWriter, IID3TagService id3TagService)
        {
            this._spotifyService = spotifyService;
            this._recordingService = recordingService;
            this._songWriter = songWriter;
            this._id3TagService = id3TagService;

            this._recordedSongs = new List<Song>();
        }

        public void Run()
        {
            this._spotifyService.GetSong().Subscribe(song =>
            {
                if (this._currentRecorder != null)
                {
                    this.StopRecording(this._currentRecorder, this._songWriter, this._id3TagService);
                }

                this._currentRecorder = song != null 
                    ? this._recordingService.StartRecording(song) 
                    : null;

                this.ReRenderScreen();
            });

            bool closeApplication = false;

            while(closeApplication == false)
            { 
                string command = System.Console.ReadLine();

                switch (command)
                {
                    case "render":
                        this.ReRenderScreen();
                        break;
                    case "clear":
                        this._recordedSongs.Clear();
                        this.ReRenderScreen();
                        break;
                    case "close":
                        closeApplication = true;
                        this._currentRecorder?.StopRecording();
                        break;
                }
            }
        }

        private void ReRenderScreen()
        {
            System.Console.Clear();

            this.RenderHeader();
            this.RenderLine();
            this.RenderStatus(this._currentRecorder);
            this.RenderLine();
            this.RenderRecordedSongs();
            this.RenderLine();
            this.RenderCommandLine();
        }

        private void RenderLine()
        {
            int width = System.Console.BufferWidth;
            Write(new string('=', width).Black().OnDarkGray());
        }

        private void RenderHeader()
        {
            List<string> header = new List<string>
            {
                @" ____                    _     _    __             ____                                     _               ",
                @"/ ___|   _ __     ___   | |_  (_)  / _|  _   _    |  _ \    ___    ___    ___    _ __    __| |   ___   _ __ ",
                @"\___ \  | '_ \   / _ \  | __| | | | |_  | | | |   | |_) |  / _ \  / __|  / _ \  | '__|  / _` |  / _ \ | '__|",
                @" ___) | | |_) | | (_) | | |_  | | |  _| | |_| |   |  _ <  |  __/ | (__  | (_) | | |    | (_| | |  __/ | |   ",
                @"|____/  | .__/   \___/   \__| |_| |_|    \__, |   |_| \_\  \___|  \___|  \___/  |_|     \__,_|  \___| |_|   ",
                @"        |_|                              |___/                                                              ",
            };

            int width = System.Console.BufferWidth;
            int textWidth = header.First().Length;

            int indent = (width - textWidth) / 2;

            foreach (var line in header)
            {
                string lineToWrite = new string(' ', indent) + line + new string(' ', indent);
                Write(lineToWrite.DarkGreen().OnGreen());
            }
        }

        private void RenderStatus(IAudioRecorder currentRecorder)
        {
            WriteLine();

            Write("  Status: ");
            WriteLine(currentRecorder != null 
                ? "Recording".DarkGreen().OnGreen() 
                : "Waiting for song".White().OnRed());

            WriteLine();

            Write("  Artist: ");
            WriteLine((currentRecorder?.Song.Artist ?? string.Empty).White());

            Write("  Song:   ");
            WriteLine((currentRecorder?.Song.Title ?? string.Empty).White());

            Write("  Album:  ");
            WriteLine((currentRecorder?.Song.Album ?? string.Empty).White());

            WriteLine();
        }

        private void RenderRecordedSongs()
        {
            WriteLine();
            WriteLine("  Aufgenommene Lieder:");
            WriteLine();

            const int RenderedCommandLineHeight = 4;
            int availableRows = System.Console.WindowHeight - System.Console.CursorTop - RenderedCommandLineHeight - 1;

            var songsToPrint = this._recordedSongs.Reverse().Take(availableRows).ToList();

            for (int i = 0; i < songsToPrint.Count; i++)
            {
                var song = songsToPrint[i];

                WriteLine($"  {i + 1}. ".White(), song.Title);
            }

            for (int i = 0; i < availableRows - this._recordedSongs.Reverse().Take(availableRows).Count(); i++)
            {
                WriteLine();
            }

            WriteLine();
        }

        private void RenderCommandLine()
        {
            WriteLine();

            Write("  Befehl: ");
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
                tags.Album = recorded.Song.Album;

                id3TagService.UpdateTags(tags, recorded);

                if (writer.WriteSong(recorded))
                {
                    this._recordedSongs.Add(recorded.Song);
                    this.ReRenderScreen();
                }
            });
        }
    }
}
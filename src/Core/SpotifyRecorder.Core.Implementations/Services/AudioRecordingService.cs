using System;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecordingService : IAudioRecordingService
    {
        private readonly ISettings _settings;

        public AudioRecordingService(ISettings settings)
        {
            this._settings = settings;
        }

        public IAudioRecorder StartRecording(Song song)
        {
            var recorder = new AudioRecorder(this._settings, song);
            recorder.StartRecording();

            return recorder;
        }
    }
}
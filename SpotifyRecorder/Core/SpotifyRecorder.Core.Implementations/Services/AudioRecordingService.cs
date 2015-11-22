using System;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecordingService : IAudioRecordingService
    {
        public IAudioRecorder StartRecording(Song song)
        {
            var recorder = new AudioRecorder(song);
            recorder.StartRecording();

            return recorder;
        }
    }
}
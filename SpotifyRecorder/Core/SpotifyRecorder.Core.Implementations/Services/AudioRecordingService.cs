using System;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecordingService : IAudioRecordingService
    {
        public IAudioRecorder StartRecording(AudioOutputDevice device, Song song)
        {
            var recorder = new AudioRecorder(device, song);
            recorder.StartRecording();

            return recorder;
        }
    }
}
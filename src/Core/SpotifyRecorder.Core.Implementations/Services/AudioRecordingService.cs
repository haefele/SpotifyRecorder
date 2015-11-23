using System;
using System.Linq;
using NAudio.CoreAudioApi;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecordingService : IAudioRecordingService
    {
        private readonly ISettings _settings;
        private readonly MMDevice _actualDevice;

        public AudioRecordingService(ISettings settings)
        {
            this._settings = settings;

            this._actualDevice = this._actualDevice = new MMDeviceEnumerator()
                .EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
                .FirstOrDefault(f => f.DeviceFriendlyName == this._settings.RecorderDeviceName);

            if (this._actualDevice == null)
                throw new SpotifyRecorderException("Recording device not found.");
        }

        public IAudioRecorder StartRecording(Song song)
        {
            var recorder = new AudioRecorder(this._settings, this._actualDevice, song);
            recorder.StartRecording();

            return recorder;
        }
    }
}
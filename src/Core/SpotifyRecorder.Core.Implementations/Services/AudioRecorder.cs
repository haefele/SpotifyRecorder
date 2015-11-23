using System.IO;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Lame;
using NAudio.Wave;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecorder : IAudioRecorder
    {
        private readonly ISettings _settings;
        private readonly MMDevice _actualDevice;
        private readonly WasapiCapture _capture;
        private readonly string _fileName;
        private readonly WaveFileWriter _writer;
        
        public Song Song { get; }

        public AudioRecorder(ISettings settings, Song song)
        {
            this._settings = settings;

            this.Song = song;

            this._actualDevice = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).First(f => f.FriendlyName == this._settings.RecorderDeviceName);

            this._capture = new WasapiCapture(this._actualDevice);
            this._capture.DataAvailable += this.CaptureOnDataAvailable;

            this._fileName = Path.GetTempFileName();
            this._writer = new WaveFileWriter(this._fileName, this._capture.WaveFormat);
        }

        public void StartRecording()
        {
            this._capture.StartRecording();
        }

        public RecordedSong StopRecording()
        {
            this._capture.StopRecording();

            this._capture.Dispose();
            this._writer.Dispose();
            
            var result =  new RecordedSong
            {
                Song = this.Song,
                Data = this.ReadFileAndConvertToMp3(),
            };

            File.Delete(this._fileName);

            if (result.Data.Any() == false)
                return null;

            return result;
        }

        private void CaptureOnDataAvailable(object sender, WaveInEventArgs e)
        {
            this._writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private byte[] ReadFileAndConvertToMp3()
        {
            using (var reader = new WaveFileReader(this._fileName))
            using (var result = new MemoryStream())
            using (var mp3Writer = new LameMP3FileWriter(result, reader.WaveFormat, (int)this._settings.BitRate))
            {
                reader.CopyTo(mp3Writer);
                return result.ToArray();
            }
        }
    }
}
using System.IO;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Lame;
using NAudio.Wave;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecorder : IAudioRecorder
    {
        private readonly MMDevice _actualDevice;
        private readonly WasapiCapture _capture;
        private readonly string _fileName;
        private readonly WaveFileWriter _writer;

        public AudioOutputDevice Device { get; }
        public Song Song { get; }

        public AudioRecorder(AudioOutputDevice device, Song song)
        {
            this.Device = device;
            this.Song = song;

            this._actualDevice = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).First(f => f.FriendlyName == device.Name);

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
            using (var mp3Writer = new LameMP3FileWriter(result, reader.WaveFormat, LAMEPreset.STANDARD))
            {
                reader.CopyTo(mp3Writer);
                return result.ToArray();
            }
        }
    }
}
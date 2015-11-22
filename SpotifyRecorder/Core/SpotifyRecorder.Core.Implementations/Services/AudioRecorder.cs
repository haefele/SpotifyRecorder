using System.IO;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class AudioRecorder : IAudioRecorder
    {
        private readonly WasapiCapture _capture;
        private readonly MemoryStream _outputStream;
        private readonly WaveFileWriter _writer;

        public Song Song { get; }

        public AudioRecorder(Song song)
        {
            this.Song = song;
            
            this._capture = new WasapiLoopbackCapture();
            this._capture.DataAvailable += this.CaptureOnDataAvailable;

            this._outputStream = new MemoryStream();
            this._writer = new WaveFileWriter(this._outputStream, this._capture.WaveFormat);
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

            return new RecordedSong
            {
                Song = this.Song,
                Data = this._outputStream.ToArray()
            };
        }

        private void CaptureOnDataAvailable(object sender, WaveInEventArgs e)
        {
            this._writer.Write(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
using System.IO;
using File = TagLib.File;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class MemoryFileAbstraction : File.IFileAbstraction
    {
        private readonly MemoryStream _stream;

        public MemoryFileAbstraction(MemoryStream stream)
        {
            this._stream = stream;

        }

        public void CloseStream(Stream stream)
        {
        }

        public string Name => "test.mp3";
        public Stream ReadStream => this._stream;
        public Stream WriteStream => this._stream;
    }
}
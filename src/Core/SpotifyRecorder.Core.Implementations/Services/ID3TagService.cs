using System.IO;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using File = TagLib.File;

namespace SpotifyRecorder.Core.Implementations.Services
{
    public class ID3TagService : IID3TagService
    {
        public ID3Tags GetTags(RecordedSong song)
        {
            var tagFile = File.Create(new MemoryFileAbstraction(new MemoryStream(song.Data)));

            return new ID3Tags
            {
                Title = tagFile.Tag.Title,
                Artists = tagFile.Tag.Performers,
                Album = tagFile.Tag.Album,
            };
        }

        public void UpdateTags(ID3Tags tags, RecordedSong song)
        {
            using (var stream = new MemoryStream(song.Data.Length * 2))
            {
                stream.Write(song.Data, 0, song.Data.Length);
                stream.Position = 0;

                var tagFile = File.Create(new MemoryFileAbstraction(stream));

                tagFile.Tag.Title = tags.Title;
                tagFile.Tag.Performers = tags.Artists;
                tagFile.Tag.Album = tags.Album;

                tagFile.Save();

                song.Data = stream.ToArray();
            }
        }
    }
}
using System.IO;
using System.Linq;
using SpotifyRecorder.Core.Abstractions.Entities;
using SpotifyRecorder.Core.Abstractions.Services;
using TagLib;
using TagLib.Id3v2;
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
                Picture = tagFile.Tag.Pictures.FirstOrDefault()?.Data.Data
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

                if (tags.Picture != null)
                {
                    tagFile.Tag.Pictures = new IPicture[]
                    {
                        new Picture(new ByteVector(tags.Picture, tags.Picture.Length)),
                    };
                }

                tagFile.Save();

                song.Data = stream.ToArray();
            }
        }
    }
}
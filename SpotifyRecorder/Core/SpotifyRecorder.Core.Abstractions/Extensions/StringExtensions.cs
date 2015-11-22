using System.IO;
using System.Linq;

namespace SpotifyRecorder.Core.Abstractions.Extensions
{
    public static class StringExtensions
    {
        public static string ToValidFileName(this string self)
        {
            return Path.GetInvalidFileNameChars().Aggregate(self, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
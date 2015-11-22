using System;
using System.Collections.Generic;

namespace SpotifyRecorder.Core.Abstractions.Entities
{
    public class Song : IEquatable<Song>
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        
        #region Equality
        public bool Equals(Song other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(this.Artist, other.Artist) && string.Equals(this.Title, other.Title);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Song)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Artist?.GetHashCode() ?? 0) * 397) ^ (this.Title?.GetHashCode() ?? 0);
            }
        }
        #endregion
    }
}
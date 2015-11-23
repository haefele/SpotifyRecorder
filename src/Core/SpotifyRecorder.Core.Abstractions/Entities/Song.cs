using System;
using System.Collections.Generic;

namespace SpotifyRecorder.Core.Abstractions.Entities
{
    public class Song : IEquatable<Song>
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        
        #region Equality
        public bool Equals(Song other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(this.Artist, other.Artist) && string.Equals(this.Title, other.Title) && string.Equals(this.Album, other.Album);
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
                var hashCode = this.Artist?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (this.Title?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (this.Album?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
        #endregion
    }
}
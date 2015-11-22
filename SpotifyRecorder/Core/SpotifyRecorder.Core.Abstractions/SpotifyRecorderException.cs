using System;
using System.Runtime.Serialization;

namespace SpotifyRecorder.Core.Abstractions
{
    [Serializable]
    public class SpotifyRecorderException : Exception
    {
        public SpotifyRecorderException()
        {
        }

        public SpotifyRecorderException(string message)
            : base(message)
        {
        }

        public SpotifyRecorderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SpotifyRecorderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
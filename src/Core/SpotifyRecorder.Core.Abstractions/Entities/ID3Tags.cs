﻿using System.Security.Policy;

namespace SpotifyRecorder.Core.Abstractions.Entities
{
    public class ID3Tags
    {
        public string[] Artists { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
    }
}
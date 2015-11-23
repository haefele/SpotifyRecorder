using SpotifyRecorder.Core.Abstractions.Settings;

namespace SpotifyRecorder.Core.Implementations.Settings
{
    public class InMemorySettings : ISettings
    {
        public BitRate BitRate { get; set; }
        public int MillisecondsToCheckForTrackChanges { get; set; }
        public string RecorderDeviceName { get; set; }
        public string OutputDirectory { get; set; }
        public bool SkipExistingSongs { get; set; }
    }
}
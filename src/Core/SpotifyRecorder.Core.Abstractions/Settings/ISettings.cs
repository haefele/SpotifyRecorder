namespace SpotifyRecorder.Core.Abstractions.Settings
{
    public interface ISettings
    {
        BitRate BitRate { get; }
        int MillisecondsToCheckForTrackChanges { get; }
        int SecondsToSkipSongsUnder { get; }
        string RecorderDeviceName { get; }
        string OutputDirectory { get; }
        bool SkipExistingSongs { get; }
    }
}
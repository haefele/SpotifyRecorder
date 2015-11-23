using AppConfigFacility;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SpotifyRecorder.Core.Abstractions.Services;
using SpotifyRecorder.Core.Abstractions.Settings;
using SpotifyRecorder.Core.Implementations.Services;

namespace SpotifyRecorder.App.Console
{
    public class DefaultWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<AppConfigFacility.AppConfigFacility>(f => f.CacheSettings());
            
            container.Register(
                Component.For<ISettings>().FromAppConfig(f => f.WithPrefix("SpotifyRecorder/")),
                Component.For<App>().LifestyleSingleton(),
                Component.For<ISpotifyService>().ImplementedBy<SpotifyService>().LifestyleSingleton(),
                Component.For<IAudioRecordingService>().ImplementedBy<AudioRecordingService>().LifestyleSingleton(),
                Component.For<ISongWriter>().ImplementedBy<SongWriter>().LifestyleSingleton(),
                Component.For<IID3TagService>().ImplementedBy<ID3TagService>().LifestyleSingleton()
            );
        }
    }
}
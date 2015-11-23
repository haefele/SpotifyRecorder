using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ColoredConsole;
using SpotifyRecorder.Core.Abstractions;
using SpotifyRecorder.Core.Abstractions.Services;
using static ColoredConsole.ColorConsole;

namespace SpotifyRecorder.App.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = new WindsorContainer();
                container.Install(FromAssembly.This());

                container.Resolve<App>().Run();

            }
            catch (Exception e)
            {
                WriteLine(e.GetBaseException().Message.Yellow().OnRed());
            }

            System.Console.ReadLine();
        }
    }
}

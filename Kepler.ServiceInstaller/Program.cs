using System;
using System.ServiceProcess;
using KeplerServiceInstaller;

namespace Kepler.ServiceInstaller
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (args != null && args.Length == 1 && args[0].Length > 1
                && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    case "install":
                    case "i":
                        SelfInstaller.InstallMe();
                        break;
                    case "uninstall":
                    case "u":
                        SelfInstaller.UninstallMe();
                        break;

                    default:
                        Console.WriteLine("Provide parameter to .exe file. Eg. KeplerServiceInstaller.exe -install (or -uninstall for deinstallation)");
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new KeplerServiceHost()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
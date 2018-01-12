using System;
using DryIoc;
using IvNetSwitcher.Core;
using IvNetSwitcher.Core.Abstractions;
using NDesk.Options;
using NLog;

namespace IvNetSwitcher
{
    internal class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private static IServices _ds;

        public static bool IsRun { get; set; }
        public static bool IsHelp { get; set; }
        public static bool IsTest { get; set; }
        public static bool IsList { get; set; }
        public static string Delay { get; set; }
        public static Uri Host { get; set; }

        public static int Main(string[] args)
        {
            try
            {
                _log.Info(@"IvNetSwitcher v{0} type -h or --help or -? to see usage", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

                var p = new OptionSet
                {
                    {"r|run", "run application", v => IsRun = v != null},
                    {"t|test", "test application", v => IsTest = v != null},
                    {"l|list", "list access points", v => IsList = v != null},
                    {"d|delay=", "{DELAY} between pings", v => Delay = v},
                    {"p|host-to-ping=", "{HOST} to ping", v => Host = new Uri(v)},
                    {"h|?|help", "show help and exit", v => IsHelp = v != null}
                };

                p.Parse(args);

                if (IsHelp)
                {
                    ShowHelp(p);
                    return (int)ExitCodes.Ok;
                }

                ConfigurationRootInit();

                _log.Info(_ds.Status());

                if (IsList)
                {
                    var rez = _ds.ListAvailableNetworks();
                    foreach (var network in rez)
                    {
                        var str = $" Network:{network.Name} Strength:{network.SignalStrength}"
                                  + (network.IsSecure
                                      ? "secure"
                                      : "insecure")
                                  + (network.IsConnected
                                      ? "connected"
                                      : "disconnected");
                        _log.Info(str);
                    }
                }

                if (IsRun) Do();

                return (int)ExitCodes.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return (int) ExitCodes.Error;
            }
        }

        private static void Do()
        {
            throw new NotImplementedException();
        }

        private static void ConfigurationRootInit()
        {
            _ds = Bootstrap.Container.Resolve<IServices>();
        }

        private static void ShowHelp(OptionSet p)
        {
            _log.Info("Usage: IvNetSwitcher [OPTIONS]+");
            _log.Info("Print documents.");
            _log.Info(string.Empty);
            _log.Info("Options:");
            p.WriteOptionDescriptions(Console.Out);

        }
    }

    public enum ExitCodes
    {
        Ok = 0,
        Error = 100,
        Unknown = 101
    }
}

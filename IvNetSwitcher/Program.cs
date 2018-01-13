using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DryIoc;
using IvNetSwitcher.Core;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Properties;
using NDesk.Options;
using NLog;

namespace IvNetSwitcher
{
    internal class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private static IWorkerService _worker;

        private static Profiles _profiles;

        #region Modifiers region

        private static bool IsRun { get; set; }
        private static bool IsHelp { get; set; }
        private static bool IsTest { get; set; }
        private static bool IsList { get; set; }
        #endregion

        #region Options region

        private static int Delay { get; set; }
        private static int Retry { get; set; }
        private static Uri HostToPing { get; set; }

        #endregion

        public static int Main(string[] args)
        {
            try
            {
                _log.Info(@"IvNetSwitcher v{0} type -h or --help or -? to see usage", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

                FillOptionsFromSettings();

                var p = new OptionSet
                {
                    {"r|run", "run application", v => IsRun = v != null},
                    {"t|test", "test application", v => IsTest = v != null},
                    {"l|list", "list access points", v => IsList = v != null},

                    {"p|host-to-ping=", "{HOST} to ping", v => HostToPing = new Uri(v)},
                    {"d|delay=", "{DELAY} between pings", v => Delay = int.Parse(v)},
                    {"a|attempts=", "{ATTEMPTS} retry between network switch", v => Retry = int.Parse(v)},

                    {"h|?|help", "show help and exit", v => IsHelp = v != null}
                };

                p.Parse(args);

                if (IsHelp)
                {
                    ShowHelp(p);
                    return (int)ExitCodes.Ok;
                }

                ConfigurationRootInit();
                LoadProfiles();

                _log.Info(_worker.GetCurrentStatus());

                if (IsList)
                {
                    var rez = _worker.ListAvailableNetworks();
                    foreach (var net in rez)
                    {
                        var str = $" Id:{net.Id} Network:{net.Name} Strength:{net.SignalStrength} "
                                  + (net.IsSecure
                                      ? "secure"
                                      : "insecure")
                                  + (net.IsConnected
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
                _log.Error(ex.Message);
                return (int) ExitCodes.Error;
            }
        }

        private static void FillOptionsFromSettings()
        {
            Delay = Settings.Default.Delay;
            HostToPing = new Uri(Settings.Default.HostToPing);
        }

        private static void LoadProfiles()
        {
            var serializer = new XmlSerializer(typeof(List<Profile>));
            using (TextReader reader = new StringReader(Settings.Default.Profiles))
            {
                _profiles = new Profiles((List<Profile>)serializer.Deserialize(reader));
            }
        }

        private static void ConfigurationRootInit()
        {
            _worker = Bootstrap.Container.Resolve<IWorkerService>();
        }

        private static void ShowHelp(OptionSet p)
        {
            _log.Info("Usage: IvNetSwitcher [OPTIONS]+");
            _log.Info("Print documents.");
            _log.Info(string.Empty);
            _log.Info("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        private static void Do()
        {
            _worker.Run(_profiles, HostToPing,  Delay, Retry);
        }
    }

    public enum ExitCodes
    {
        Ok = 0,
        Error = 100,
        Unknown = 101
    }
}

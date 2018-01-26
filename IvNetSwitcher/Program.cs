using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DryIoc;
using IvNetSwitcher.Core;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.Dto;
using IvNetSwitcher.Core.Shared;
using IvNetSwitcher.Properties;
using IvNetSwitcher.Shared;
using NDesk.Options;
using NLog;

namespace IvNetSwitcher
{
    internal class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private static INetService _net;
        private static IAppService _appSvc;

        private static Profiles _profiles;
        
        #region Modifiers region

        private static bool IsRun { get; set; }
        private static bool IsHelp { get; set; }
        private static bool IsTest { get; set; }
        private static bool IsList { get; set; }
        public static string ToEncrypt { get; set; }

        #endregion

        #region Options region

        private static int DelayInSec { get; set; } = 30;
        private static int Retry { get; set; } = 3;
        private static int MaxTimesToCheck { get; set; } = 0;
        private static Uri HostToPing { get; set; } = new Uri("http://www.google.com");

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
                    {"e|encrypt=", "encrypt {STRING} to password", v => ToEncrypt = v},

                    {"p|host-to-ping=", "{HOST} to ping, [default http://www.google.com]", v => HostToPing = new Uri(v)},
                    {"d|delay=", "{DELAY} between pings in seconds, [default 30 second]", v => DelayInSec = int.Parse(v)},
                    {"a|attempts=", "{ATTEMPTS} retry between network switch [default 3]", v => Retry = int.Parse(v)},
                    {"m|max-checks=", "{TIMES} to check networks. [default 0, unlimited]", v => MaxTimesToCheck = int.Parse(v)},

                    {"h|?|help", "show help and exit", v => IsHelp = v != null}
                };

                p.Parse(args);

                if (IsHelp)
                {
                    ShowHelp(p);
                    return (int)ExitCodes.Ok;
                }

                if(!string.IsNullOrWhiteSpace(ToEncrypt))
                {
                    Console.WriteLine(Utils.GetEncryptedString(ToEncrypt, Settings.Default.EncSalt));
                    return (int)ExitCodes.Ok;
                }
                
                ConfigurationRootInit();
                LoadData();
                
                _log.Info(_net.Status());

                if (IsList)
                {
                    var rez = _net.ListAvailableNetworks();
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
            DelayInSec = Settings.Default.Delay;
            HostToPing = new Uri(Settings.Default.HostToPing);
        }

        private static void LoadData()
        {
            var tmpProfiles = Settings.Default.Profiles.XmlDeserializeFromString<List<ProfileDto>>();
            _profiles = _appSvc.LoadData(new Profiles(_net, tmpProfiles, Settings.Default.EncSalt));
        }

        private static void ConfigurationRootInit()
        {
            _appSvc = Bootstrap.Container.Resolve<IAppService>();
            _net = Bootstrap.Container.Resolve<INetService>();
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
            LoadData();
            _appSvc.Run(HostToPing,  DelayInSec, Retry, MaxTimesToCheck);
        }
    }
}

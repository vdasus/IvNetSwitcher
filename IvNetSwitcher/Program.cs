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
        public static string Delay { get; set; }
        public static Uri Host { get; set; }

        private static void Main(string[] args)
        {
            try
            {
                var p = new OptionSet
                {
                    {"r|run", "run application", v => IsRun = v != null},
                    {"t|test", "test application", v => IsTest = v != null},
                    {"d|delay=", "{DELAY} between pings", v => Delay = v},
                    {"h|host=", "{HOST} to ping", v => Host = new Uri(v)},
                    {"h|?|help", "show help and exit", v => IsHelp = v != null}
                };

                p.Parse(args);

                ConfigurationRootInit();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private static void ConfigurationRootInit()
        {
            _ds = Bootstrap.Container.Resolve<IServices>();
        }
    }
}

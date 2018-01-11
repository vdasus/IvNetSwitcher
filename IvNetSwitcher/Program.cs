using DryIoc;
using IvNetSwitcher.Core;
using IvNetSwitcher.Core.Abstractions;
using NDesk.Options;

namespace IvNetSwitcher
{
    internal class Program
    {
        private static IServices _ds;
        public static bool IsRun { get; set; }
        public static bool IsHelp { get; set; }

        private static void Main(string[] args)
        {
            _ds = Bootstrap.Container.Resolve<IServices>();

            var p = new OptionSet
            {
                {"r|run", "run application", v => IsRun = v != null},
                {"h|?|help", "show help and exit", v => IsHelp = v != null}
            };

            p.Parse(args);

        }
    }
}

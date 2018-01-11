using NDesk.Options;

namespace IvNetSwitcher
{
    internal class Program
    {
        public static bool IsRun { get; set; }
        public static bool IsHelp { get; set; }

        private static void Main(string[] args)
        {
            var p = new OptionSet
            {
                {"r|run", "run application", v => IsRun = v != null},
                {"h|?|help", "show help and exit", v => IsHelp = v != null}
            };

            p.Parse(args);

        }
    }
}

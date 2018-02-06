using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace IvNetSwitcher.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string APP_GUID = "730DE481-2077-4D59-AF65-543669121202";
        private static Mutex _instanceMutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            string mutexId = $"Global\\{{{APP_GUID}}}";

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                MutexRights.FullControl, AccessControlType.Allow);

            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);

            _instanceMutex = new Mutex(true, mutexId, out var createdNew, securitySettings);
            if (!createdNew)
            {
                MessageBox.Show("Another Instance of the application is already running.", "Alert", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                _instanceMutex = null;
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _instanceMutex?.ReleaseMutex();
            base.OnExit(e);
        }
    }
}

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using IvNetSwitcher.UI.Shared;
using NLog;

namespace IvNetSwitcher.UI.ViewModel
{
    [Magic]
    public class MainViewModel
    {
        private const string CWAIT_MSG = "Wait, please...";
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public string Caption { get; } =
            $"IvNetSwitcher v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";


        public bool IsBusy { get; set; }

        public RelayCommand SettingsCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand MinMaxCommand { get; set; }

        public string StatusText { get; private set; }
        

        public MainViewModel()
        {
            InitCommands();
        }

        // TODO just temp snippets
        /*
                Messenger.Default.Send(new NotificationMessage(this,
                    $"{_currentActiveWorkItemInfo.Id} CompletedWork update successful",
                "ShowTooltip"));

                Messenger.Default.Send(new NotificationMessage(this, rezAll.Error, "ShowErrorTooltip"));
        */
        
        private void InitCommands()
        {
            MinMaxCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ToggleWindow"));
            });

            SettingsCommand = new RelayCommand(() =>
            {

            });
            HelpCommand = new RelayCommand(() =>
            {

            });
            RefreshCommand = new RelayCommand(() =>
            {

            });
        }

        private void InformAboutError(string errorString)
        {
            _log.Error(errorString);
            Messenger.Default.Send(new NotificationMessage(this, errorString, "ShowErrorTooltip"));
            StatusText = errorString;
        }

        private void SetBusyIndicator(bool isOn = true, string message = CWAIT_MSG)
        {
            if (isOn)
            {
                StatusText = message;
                IsBusy = true;
            }
            else
            {
                StatusText = (StatusText.Contains(CWAIT_MSG)) ? string.Empty : StatusText;
                IsBusy = false;
            }
        }
    }
}

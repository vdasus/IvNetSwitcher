using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using IvNetSwitcher.UI.Shared;

namespace IvNetSwitcher.UI.ViewModel
{
    [Magic]
    public class MainViewModel
    {
        private const string CWAIT_MSG = "Wait, please...";
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

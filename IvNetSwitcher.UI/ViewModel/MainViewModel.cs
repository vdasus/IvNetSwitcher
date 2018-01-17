using GalaSoft.MvvmLight.Command;
using IvNetSwitcher.UI.Shared;

namespace IvNetSwitcher.UI.ViewModel
{
    [Magic]
    public class MainViewModel
    {
        public RelayCommand SettingsCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }

        public string StatusText { get; private set; }
        

        public MainViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
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
    }
}

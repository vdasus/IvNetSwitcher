using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.Dto;
using IvNetSwitcher.Core.Shared;
using IvNetSwitcher.UI.Properties;
using IvNetSwitcher.UI.Shared;
using NLog;

namespace IvNetSwitcher.UI.ViewModel
{
    [Magic]
    public class MainViewModel: PropertyChangedBase
    {
        private const string CWAIT_MSG = "Wait, please...";
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly INetService _net;

        public string Caption { get; } =
            $"IvNetSwitcher v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        
        public bool IsBusy { get; set; }

        #region Commands region

        public RelayCommand MinMaxCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand SettingsCommand { get; set; }

        #endregion

        public string StatusText { get; private set; }

        public int SelectedTabIndex { get; set; }
        public bool IsSettingsOpened { get; set; }
        public bool IsHelpOpened { get; set; }
        
        public Profiles RegisteredNets { get; set; }
        public Profiles AvailableNets { get; set; }
        
        public MainViewModel(INetService net)
        {
            _net = net;
            InitCommands();
            LoadData();
        }

        private void LoadData()
        {
            var tmpProfiles = Settings.Default.Profiles.XmlDeserializeFromString<List<ProfileDto>>();
            RegisteredNets = new Profiles(_net, tmpProfiles, Settings.Default.EncSalt);
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

            RefreshCommand = new RelayCommand(() =>
            {

            });

            SettingsCommand = new RelayCommand(() => { IsSettingsOpened = !IsSettingsOpened; });

            HelpCommand = new RelayCommand(() => { IsHelpOpened = !IsHelpOpened; });
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

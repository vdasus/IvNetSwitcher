using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IAppService _appSvc;
        private readonly IUtilsService _svc;

        private bool _isMonitoring;

        public string Caption { get; } =
            $"IvNetSwitcher v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        
        public bool IsBusy { get; set; }

        #region Commands region

        public RelayCommand MinMaxCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand AddProfileCommand { get; set; }
        public RelayCommand GoPlayCommand { get; set; }
        public RelayCommand GoStopCommand { get; set; }
        public RelayCommand GoNextCommand { get; set; }
        public RelayCommand EditProfileCommand { get; set; }
        public RelayCommand DeleteProfileCommand { get; set; }
        public RelayCommand RefreshNetworksCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand<int> PressTileCommand { get; set; }

        #endregion

        public string StatusText { get; private set; }

        public int SelectedTabIndex { get; set; }
        public bool IsSettingsOpened { get; set; }
        public bool IsHelpOpened { get; set; }
        public bool IsRegisterOpened { get; set; }

        #region Settings region

        public Profiles RegisteredNets { get; set; }
        public IReadOnlyList<Network> AvailableNets { get; set; }
        public Network SelectedNet { get; set; }

        public string SettingsUserName { get; set; }
        public string SettingsPwd { get; set; }
        
        #endregion
        
        public MainViewModel(INetService net, IAppService appSvc, IUtilsService svc)
        {
            _net = net;
            _appSvc = appSvc;
            _svc = svc;
            _svc.SetSalt(Settings.Default.EncSalt);

            _isMonitoring = false;

            InitCommands();
            LoadData();
        }

        private void LoadData()
        {
            var tmpProfiles = Settings.Default.Profiles.XmlDeserializeFromString<List<ProfileDto>>();
            RegisteredNets = _appSvc.LoadData(new Profiles(_net, _svc, tmpProfiles));
        }

        // TODO just temp snippets
        /*
                Messenger.Default.Send(new NotificationMessage(this, $"", "ShowTooltip"));
                Messenger.Default.Send(new NotificationMessage(this, rezAll.Error, "ShowErrorTooltip"));
        */

        private void InitCommands()
        {
            MinMaxCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ToggleWindow"));
            });

            RefreshCommand = new RelayCommand(LoadData);

            SettingsCommand = new RelayCommand(() => { IsSettingsOpened = !IsSettingsOpened; });

            HelpCommand = new RelayCommand(() => { IsHelpOpened = !IsHelpOpened; });

            RegisterCommand = new RelayCommand(async () =>
            {
                IsRegisterOpened = !IsRegisterOpened;
                if(IsRegisterOpened) await LoadAvailableNetworks();
            });

            AddProfileCommand = new RelayCommand(() =>
            {
                
            });

            GoPlayCommand = new RelayCommand(async () =>
            {
                await Task.Run(() =>
                {
                    _isMonitoring = true;
                    SetBusyIndicator(true, "Monitoring ...");
                    while (_isMonitoring)
                    {
                        var go = _appSvc.Run(new Uri(Settings.Default.HostToPing), Settings.Default.Retry);
                        if (go.IsFailure)
                        {
                            SetBusyIndicator(false, go.Error);
                            break;
                        };
                        Thread.Sleep(TimeSpan.FromSeconds(Settings.Default.DelayInSec));
                    }
                    SetBusyIndicator(false);
                });
            });

            GoStopCommand = new RelayCommand(() =>
            {
                _isMonitoring = false;
                StatusText = CWAIT_MSG;
            });

            GoNextCommand = new RelayCommand(() =>
            {
                var rez = _appSvc.GoNext();
                if (rez.IsFailure) StatusText = rez.Error;
            });

            EditProfileCommand = new RelayCommand(() => { });
            DeleteProfileCommand = new RelayCommand(() => { });
            SaveSettingsCommand = new RelayCommand(() =>
            {
                // TODO domain and comment
                // TODO UI password input
                _appSvc.RegisterProfile(new Profile(_net, _svc, SelectedNet.Id, SelectedNet.Name, SettingsUserName, SettingsPwd, "", "", true));

                try
                {
                    List<ProfileDto> profiles = _appSvc.GetProfilesDtos().ToList();
                    Settings.Default.Profiles = profiles.SerializeToXmlStringIndented();
                    Settings.Default.Save();
                    LoadData();
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                    StatusText = ex.Message;
                }
            });

            RefreshNetworksCommand = new RelayCommand(async () => { await LoadAvailableNetworks(); });
            PressTileCommand = new RelayCommand<int>((e) => { StatusText = e.ToString(); });
        }

        private async Task LoadAvailableNetworks()
        {
            try
            {
                SetBusyIndicator();
                await RunLoadNetworksAsync();
            }
            finally
            {
                SetBusyIndicator(false);
            }
        }

        private async Task RunLoadNetworksAsync() => 
            await Task.Run(() =>
            {
                AvailableNets = _appSvc.GetNetworks();
            });

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
                Messenger.Default.Send(new NotificationMessage(this, "", "SetActiveTray"));
            }
            else
            {
                StatusText = StatusText.Contains(CWAIT_MSG) ? string.Empty : StatusText;
                IsBusy = false;
                Messenger.Default.Send(new NotificationMessage(this, "", "SetNormalTray"));
            }
        }
    }
}

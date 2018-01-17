using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace IvNetSwitcher.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _mIsExplicitClose;

        public MainWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case "ShowTooltip":
                    //https://www.codeproject.com/Articles/36468/WPF-NotifyIcon
                    NotifyIcon.ShowBalloonTip("gtemper", $"{msg.Target}", BalloonIcon.Info);
                    break;
                case "SetActiveTray":
                    NotifyIcon.IconSource = new BitmapImage(new Uri(
                        "pack://application:,,,/gtemper;component/Resources/IvNetSwitcher_active.ico",
                        UriKind.Absolute));
                    break;

                case "SetNormalTray":
                    NotifyIcon.IconSource = new BitmapImage(new Uri(
                        "pack://application:,,,/gtemper;component/Resources/IvNetSwitcher.ico",
                        UriKind.Absolute));
                    break;
                case "ToggleWindow":
                    if (!IsVisible)
                        Show();
                    else
                        Hide();

                    if (WindowState == WindowState.Minimized)
                        WindowState = WindowState.Normal;

                    Activate();
                    Topmost = true;
                    Topmost = false;
                    Focus();
                    break;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Minimized:
                    Hide();
                    break;
                default:
                    Show();
                    Dispatcher.BeginInvoke(new Action(() => Activate()),
                        System.Windows.Threading.DispatcherPriority.ContextIdle, null);
                    break;
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_mIsExplicitClose) return;
            e.Cancel = true;
            Hide();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                /*Settings.Default.WinWidth = Width;
                Settings.Default.WinHeight = Height;
                Settings.Default.Save();*/
            }
            catch
            {
                //
            }

            _mIsExplicitClose = true;
            Close();
        }
    }
}

using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using Wpf.Ui.TaskBar;
using XHS.Common.Global;
using XHS.Common.Utils;
using XHS.Spider.Helpers;
using XHS.Spider.Services;

namespace XHS.Spider.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        private readonly TaskbarIcon _notifyIcon;
        private ContextMenu _contextMenu;
        private UpdateCheckerServer updateChecker;
        private bool _initialized = false;
        private readonly ITaskBarService _taskBarService;
        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageServiceNew pageService, ITaskBarService taskBarService, INavigationService navigationService, ISnackbarService snackbarService)
        {
            _taskBarService = taskBarService;
            ViewModel = viewModel;
            DataContext = this;
            #region 通知
            _notifyIcon = new TaskbarIcon();
            _notifyIcon.TrayBalloonTipClicked += notifyIcon_TrayBalloonTipClicked;
            updateChecker = new UpdateCheckerServer();
            updateChecker.NewVersionFound += updateChecker_NewVersionFound;
            updateChecker.NewVersionNotFound += updateChecker_NewVersionNotFound;
            #endregion


            InitializeComponent();
            SetPageService(pageService);
            navigationService.SetNavigationControl(RootNavigation);
            snackbarService.SetSnackbarControl(RootSnackbar);
            Loaded += (_, _) => InvokeSplashScreen();
        }
        #region 通知
        private void updateChecker_NewVersionFound(object sender, EventArgs e)
        {
            Application.Current.Dispatcher?.InvokeAsync(() =>
            {
                if (updateChecker.Found)
                {
                    _notifyIcon.ShowBalloonTip(
                                string.Format("{0}{1}更新",
                                        UpdateCheckerServer.Name, updateChecker.LatestVersionNumber),
                                "点击下载新版本", BalloonIcon.Info);
                }
            });
        }

        private void updateChecker_NewVersionNotFound(object sender, EventArgs e)
        {
            _notifyIcon.ShowBalloonTip($@"{UpdateCheckerServer.Name} {UpdateCheckerServer.FullVersion}",
            $@"没有找到新版本{Environment.NewLine}{UpdateCheckerServer.Version}≥{updateChecker.LatestVersionNumber}",
            BalloonIcon.Info);
        }

        private void UpdateChecker_NewVersionFoundFailed(object sender, EventArgs e)
        {
            _notifyIcon.ShowBalloonTip($@"{UpdateCheckerServer.Name} {UpdateCheckerServer.FullVersion}","检查更新失败", BalloonIcon.Info);
        }
        private void notifyIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
        {
            var url = updateChecker.LatestVersionUrl;
            if (!string.IsNullOrWhiteSpace(url))
            {
                Utils.OpenURL(url);
            }
        }
        #endregion

        private void InvokeSplashScreen()
        {
            if (_initialized)
                return;

            _initialized = true;

            RootMainGrid.Visibility = Visibility.Collapsed;
            RootWelcomeGrid.Visibility = Visibility.Visible;

            _taskBarService.SetState(this, TaskBarProgressState.Indeterminate);
            Task.Run(async () =>
            {
                //TODO:这里预留程序启动初始化数据
                //await Task.Delay(2000);
                updateChecker.Check(true);
                await Dispatcher.InvokeAsync(() =>
                {
                    RootWelcomeGrid.Visibility = Visibility.Hidden;
                    RootMainGrid.Visibility = Visibility.Visible;

                    Navigate(typeof(Pages.DashboardPage));

                    _taskBarService.SetState(this, TaskBarProgressState.None);
                });

                return true;
            });
        }
        #region INavigationWindow methods

        public Frame GetFrame()
            => RootFrame;

        public INavigation GetNavigation()
            => RootNavigation;

        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods

        
        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (GlobalCaChe.clipboardHooker != null)
            {
                GlobalCaChe.clipboardHooker.Dispose();
            }
            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Navigated to: {sender?.Current ?? null}", "Wpf.Ui.Demo");

            // This funky solution allows us to impose a negative
            // margin for Frame only for the Dashboard page, thanks
            // to which the banner will cover the entire page nicely.
            RootFrame.Margin = new Thickness(
                left: 0,
                top: sender?.Current?.PageTag == "dashboard" ? -69 : 0,
                right: 0,
                bottom: 0);
        }
    }
}
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using XHS.Common.Events;
using XHS.Common.Global;
using XHS.Common.Helpers;
using XHS.Common.Utils;
using XHS.Spider.Helpers;
using XHS.Spider.Services;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        private ScriptHost scriptHost = null;
        private readonly TaskbarIcon _notifyIcon;
        private ContextMenu _contextMenu;
        private UpdateCheckerServer updateChecker;
        private bool _initialized = false;
        private readonly ITaskBarService _taskBarService;
        private readonly IPageServiceNew _pageServiceNew;
        private readonly IServiceProvider _serviceProvider;
        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageServiceNew pageService, ITaskBarService taskBarService, INavigationService navigationService, IServiceProvider serviceProvider, ISnackbarService snackbarService)
        {
           
            _taskBarService = taskBarService;
            _serviceProvider = serviceProvider;
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
            webView.Source = new Uri("https://www.xiaohongshu.com/explore");
            InitializeAsync();
            SetPageService(pageService);
            _pageServiceNew = pageService;
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
        #region webView
        private async void InitializeAsync()
        {

            //webView = this.webView;
            await webView.EnsureCoreWebView2Async(null);
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            //WebViewHandler.OnSubscribeGetXsXtHandler += GetXsXtEvent;
        }
        private async Task<string> GetXsXtEvent(object sender, string e)
        {
            string url = e;
            string jscode = "var url='" + url + "';\r\n" + @"try {
                                                                sign(url);
                                                            } catch (e) { winning.log(e); }
                                                            function sign(url) {
                                                                var t;
                                                                var o = window._webmsxyw(url, t);
                                                                return o;
                                                            }";
            var xsxtStr = await this.webView.CoreWebView2.ExecuteScriptAsync(jscode);
            return xsxtStr;
        }
        /// <summary>
        /// 加载完页面时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //TODO:这里处理加载完成后自动获取cookie设置，放到后面处理
        }
        #endregion
        private void InvokeSplashScreen()
        {
            scriptHost = ScriptHost.GetScriptHost(webView);
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
            var currentTag = sender?.Current?.PageTag;
            switch (currentTag)
            {
                case "search":
                    _pageServiceNew.Scope = _serviceProvider.CreateScope();
                    var dc = _pageServiceNew.Scope.ServiceProvider.GetRequiredService<SearchViewModel>();
                    dc.webView = webView;
                    break;
                default:
                    break;
            }
            //_navigationService.GetNavigationControl().Current.PageTag;
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
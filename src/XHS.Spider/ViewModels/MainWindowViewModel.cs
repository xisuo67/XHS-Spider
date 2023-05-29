using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Global;
using XHS.Common.Helpers;
using XHS.Models.Business;
using XHS.Models.XHS.ApiOutputModel.Search.BusinessModel;

namespace XHS.Spider.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private UserInfoModel _currentUser = new UserInfoModel();

        public UserInfoModel? CurrentUser {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }
        private void InitCurrentUser() {
            if (GlobalCaChe.CurrentUser!=null)
            {
                GlobalCaChe.CurrentUser.HeadImage=FileHelper.UrlToBitmapImage(GlobalCaChe.CurrentUser.HeadUrl);
                CurrentUser =GlobalCaChe.CurrentUser;
            }
            else
            {
                var image = DrawHealper.CreateHead("未登录");
                var bit= FileHelper.BitmapToBitmapImage(image);
                CurrentUser.HeadImage = bit;
            }
        }
        private void InitializeViewModel()
        {
            ApplicationTitle = "小红书数据采集工具";
            InitCurrentUser();
            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "首页",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                new NavigationItem()
                {
                    Content = "搜索",
                    PageTag = "search",
                    Icon = SymbolRegular.Search12,
                    PageType = typeof(Views.Pages.Search)
                },
                new NavigationItem()
                {
                    Content = "Cookie",
                    PageTag = "cookie",
                    Icon = SymbolRegular.DataHistogram24,
                    PageType = typeof(Views.Pages.SettingCookie)
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "设置",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }

    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Utils;
using XHS.IService;
using XHS.Spider.Views.Windows;

namespace XHS.Spider.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IWindowService _windowService;

        private ICommand _navigateCommand;

        private ICommand _openWindowCommand;
        public DashboardViewModel(INavigationService navigationService, IWindowService windowService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
        }
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

        public ICommand OpenWindowCommand => _openWindowCommand ??= new RelayCommand<string>(OnOpenWindow);
        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }
        private void OnNavigate(string parameter)
        {
            switch (parameter)
            {
                case "navigate_to_settingCookie":
                    _navigationService.Navigate(typeof(Views.Pages.SettingCookie));
                    return;
                case "navigate_to_search":
                    _navigationService.Navigate(typeof(Views.Pages.Search));
                    return;
                case "navigate_to_QQ":
                    string qqUrl = "https://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=Kdj_m0z41zV0IrR5rSD1DPfv80KcEw_d&authKey=TCEQAhosZjagfkCcfNJyrkCXWFkwiSEJNJgNZTYXBADYOy6THzjN4GrOdxOndjII&noverify=0&group_code=521302423";
                    Utils.OpenURL(qqUrl);
                    return;
                case "navigate_to_HomePage":
                    string homeUrl = "https://xisuo67.website/XHS-Spider-Doc/";
                    Utils.OpenURL(homeUrl);
                    return;
            }
        }

        private void OnOpenWindow(string parameter)
        {
            switch (parameter)
            {
                case "open_window_ScanLogin":
                    _windowService.Show<ScanLogin>();
                    return;
            }
        }
    }
}

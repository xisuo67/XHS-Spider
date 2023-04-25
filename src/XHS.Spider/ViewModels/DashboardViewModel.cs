using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.IService;

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
                    _navigationService.Navigate(typeof(Views.Pages.DataPage));
                    return;
            }
        }

        private void OnOpenWindow(string parameter)
        {
            switch (parameter)
            {
                case "open_window_store":
                    //_windowService.Show<Views.Windows.StoreWindow>();
                    return;

                case "open_window_manager":
                    //_windowService.Show<Views.Windows.TaskManagerWindow>();
                    return;

                case "open_window_editor":
                    //_windowService.Show<Views.Windows.EditorWindow>();
                    return;

                case "open_window_settings":
                    //_windowService.Show<Views.Windows.SettingsWindow>();
                    return;

                case "open_window_experimental":
                    //_windowService.Show<Views.Windows.ExperimentalWindow>();
                    return;
            }
        }
    }
}

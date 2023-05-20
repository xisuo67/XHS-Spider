using System.Windows;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Global;
using XHS.IService;
using XHS.Models.SettingCookie;
using XHS.Spider.ViewModels;
using XHS.Spider.Views.Windows;

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// SettingCookie.xaml 的交互逻辑
    /// </summary>
    public partial class SettingCookie : INavigableView<SettingCookieViewModel>
    {
        private readonly IWindowService _windowService;
        private readonly ISnackbarService _snackbarService;
        public SettingCookieViewModel ViewModel { get; }
        public SettingCookie(SettingCookieViewModel viewModel, IWindowService windowService, ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
            ViewModel = viewModel;
            _windowService = windowService;
            InitializeComponent();
        }

        private void CookieSetting_Click(object sender, RoutedEventArgs e)
        {
            //if (GlobalCaChe.Cookies.Count > 0)
            //{
            //    _snackbarService.Show("提示", "无法设置多个cookie", SymbolRegular.ErrorCircle12, ControlAppearance.Danger);
            //    return;
            //}
            CookieEdit cookie = new CookieEdit();
            cookie.ShowDialog();
            ViewModel.InitializeData();

        }

        private void CookieEdit_Click(object sender, RoutedEventArgs e)
        {
            CookieModel selectedRow = (CookieModel)dgrdView.SelectedItem;
            CookieEdit cookie = new CookieEdit(selectedRow);
            cookie.ShowDialog();
            ViewModel.InitializeData();
        }

        private void CookieDelete_Click(object sender, RoutedEventArgs e)
        {
            CookieModel selectedRow = (CookieModel)dgrdView.SelectedItem;
            GlobalCaChe.Cookies.Remove(selectedRow);
            ViewModel.InitializeData();
        }
    }
}

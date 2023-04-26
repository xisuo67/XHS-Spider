using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Interfaces;
using XHS.IService;
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
        public SettingCookieViewModel ViewModel { get; }
        public SettingCookie(SettingCookieViewModel viewModel, IWindowService windowService)
        {
            ViewModel = viewModel;
            _windowService = windowService;
            InitializeComponent();
        }

        private void CookieSetting_Click(object sender, RoutedEventArgs e)
        {
            _windowService.Show<CookieEdit>();
        }
    }
}

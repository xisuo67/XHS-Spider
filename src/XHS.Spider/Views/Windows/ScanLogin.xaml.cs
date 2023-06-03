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
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Views.Windows
{
    /// <summary>
    /// ScanLogin.xaml 的交互逻辑
    /// </summary>
    public partial class ScanLogin : Window, INavigationWindow
    {
        public ScanLoginViewModel ViewModel
        {
            get;
        }
        public ScanLogin(ScanLoginViewModel viewModel)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
            this.ViewModel= viewModel;
            InitializeComponent();
        }

        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public Frame GetFrame()
        {
            throw new NotImplementedException();
        }

        public INavigation GetNavigation()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow()
        {
            throw new NotImplementedException();
        }
    }
}

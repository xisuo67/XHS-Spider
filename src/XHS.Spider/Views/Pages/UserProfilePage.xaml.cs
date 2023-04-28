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

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// UserProfilePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserProfilePage : INavigableView<ViewModels.UserProfileViewModel>
    {
        public ViewModels.UserProfileViewModel ViewModel
        {
            get;
        }

        public UserProfilePage(ViewModels.UserProfileViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}

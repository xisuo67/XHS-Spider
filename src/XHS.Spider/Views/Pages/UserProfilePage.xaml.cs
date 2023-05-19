using Microsoft.Web.WebView2.Wpf;
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
using Wpf.Ui.TaskBar;
using XHS.Spider.Helpers;

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// UserProfilePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserProfilePage : INavigableView<ViewModels.UserProfileViewModel>
    {
        //private ScriptHost scriptHost = null;
     
        public ViewModels.UserProfileViewModel ViewModel
        {
            get;
        }

        public UserProfilePage(ViewModels.UserProfileViewModel viewModel)
        {
            viewModel.InitNullImage();
            ViewModel = viewModel;
            InitializeComponent();
        }
        private void CheckAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.dgrdView.Items.Count; i++)
            {
                var cntr = dgrdView.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = dgrdView.Columns[0].GetCellContent(ObjROw);
                    if (objElement != null)
                    {
                        System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
                        if (objChk.IsChecked == false)
                        {
                            objChk.IsChecked = true;
                        }
                        else
                        {
                            objChk.IsChecked = false;
                        }
                    }
                }
            }
        }

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            //scriptHost = ScriptHost.GetScriptHost(webView);
        }
    }
}

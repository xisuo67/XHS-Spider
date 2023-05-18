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
        private ScriptHost scriptHost = null;
     
        public ViewModels.UserProfileViewModel ViewModel
        {
            get;
        }

        public UserProfilePage(ViewModels.UserProfileViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            InitializeAsync();
            //viewModel.webView = this.webView;
        }
        #region webView
        private async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
        }
        //private async Task<string> GetXsXtEvent(object sender, string e)
        //{
        //    string url = e;
        //    string jscode = "var url='" + url + "';\r\n" + @"try {
        //                                                        sign(url);
        //                                                    } catch (e) { winning.log(e); }
        //                                                    function sign(url) {
        //                                                        var t;
        //                                                        var o = window._webmsxyw(url, t);
        //                                                        return o;
        //                                                    }";
        //    var xsxtStr = await this.webView.CoreWebView2.ExecuteScriptAsync(jscode);
        //    return xsxtStr;
        //}
        #endregion
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

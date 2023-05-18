using Microsoft.Web.WebView2.Core;
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
using Wpf.Ui.Controls;
using XHS.Common.Global;
using XHS.Spider.Helpers;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : INavigableView<ViewModels.SearchViewModel>
    {
     
        public ViewModels.SearchViewModel ViewModel
        {
            get;
        }

        public Search(ViewModels.SearchViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
          
            InitializeAsync();
        }

        #region webView
        private async void InitializeAsync()
        {

            //ViewModel.webView = this.webView;
            //await webView.EnsureCoreWebView2Async(null);
            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
        }
        #endregion

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //scriptHost = ScriptHost.GetScriptHost(webView);
        }

    }
}

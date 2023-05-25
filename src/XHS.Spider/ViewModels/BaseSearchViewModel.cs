using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 基础搜索类
    /// </summary>
    public abstract class BaseSearchViewModel : ObservableObject
    {
        public WebView2 webView;

        private string inputText;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }

        public abstract void ExecuteInitData();
    }
}

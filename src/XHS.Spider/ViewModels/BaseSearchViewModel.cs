using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 基础搜索类
    /// </summary>
    public abstract class BaseSearchViewModel : ObservableObject
    {
        /// <summary>
        /// 搜索栏输入框
        /// </summary>
        private string inputText;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }

        // 输入确认事件
        private ICommand inputCommand;
        public ICommand InputCommand
        {
            get => inputCommand ?? (inputCommand = new Wpf.Ui.Common.RelayCommand(ExecuteInitData));
            set => inputCommand = value;
        }
        /// <summary>
        /// 执行数据初始化
        /// </summary>
        public abstract void ExecuteInitData();
    }
}

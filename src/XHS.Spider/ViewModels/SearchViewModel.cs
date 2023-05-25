using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Global;
using XHS.Common.Helpers;
using XHS.Common.Utils;
using XHS.IService.XHS;
using XHS.Models.Enum;
using XHS.Service.Log;
using XHS.Service.XHS;
using XHS.Spider.Services;
using System.IO;
using XHS.Common.Events;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 搜索
    /// </summary>
    public partial class SearchViewModel : ObservableObject, INavigationAware
    {
        public  WebView2 webView;
        private static readonly ILogger Logger = LoggerService.Get(typeof(SearchViewModel));
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPageServiceNew _pageServiceNew;
        private readonly ISnackbarService _snackbarService;
        private readonly IEventAggregator _aggregator;
        public SearchViewModel(
            IServiceProvider serviceProvider,
            IPageServiceNew pageServiceNew,
            ISnackbarService snackbarService,
            INavigationService navigationService,
            IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            _snackbarService = snackbarService;
            _serviceProvider = serviceProvider;
            _pageServiceNew = pageServiceNew;
            _navigationService = navigationService;
           
        }
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
            get => inputCommand ?? (inputCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(ExecuteInput));
            set => inputCommand = value;
        }
        /// <summary>
        /// 处理输入事件
        /// </summary>
        private async void ExecuteInput()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (GlobalCaChe.Cookies.Count == 0)
                {
                    _snackbarService.Show("提示", "请先设置cookie", SymbolRegular.ErrorCircle12, ControlAppearance.Danger);
                    return;
                }
                var navigation = _navigationService.GetNavigationControl();
                //TODO:搜索服务，跳转对应页面
                try
                {
                    var searchService= SearchService.GetSearchService(webView, _aggregator,navigation, _serviceProvider, _pageServiceNew);
                    searchService.SearchInput(InputText);
                    this.InputText = string.Empty;
                }
                catch (Exception ex)
                {
                    Logger.Error("跳转异常：", ex);
                }

            }
        }
        #region 剪贴板

        private int times = 0;

        /// <summary>
        /// 监听剪贴板更新事件，会执行两遍以上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClipboardUpdated(object sender, EventArgs e)
        {

            var currentTag= _navigationService.GetNavigationControl().Current.PageTag;
            if (currentTag!= "search")
            {
                return;
            }
            #region 执行第二遍时跳过
            times += 1;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 300)
            };
            timer.Tick += (s, ex) => { timer.IsEnabled = false; times = 0; };
            timer.IsEnabled = true;

            if (times % 2 == 0)
            {
                timer.IsEnabled = false;
                times = 0;
                return;
            }

            #endregion
            //TODO:这里后面改成系统设置，暂时写死
            AllowStatus isListenClipboard = AllowStatus.YES;
            if (isListenClipboard != AllowStatus.YES)
            {
                return;
            }

            string input;
            try
            {
                IDataObject data = System.Windows.Clipboard.GetDataObject();
                string[] fs = data.GetFormats();
                input = data.GetData(fs[0]).ToString();
                if (input.Contains("www.xiaohongshu.com"))
                {
                    //TODO:验证input是否url
                    this.InputText = input;
                    ExecuteInput();
                }

            }
            catch (Exception exc)
            {
                Logger.Error("OnClipboardUpdated", exc);
                return;
            }
           
        }

        #endregion
        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            GlobalCaChe.clipboardHooker = new ClipboardHooker(Application.Current.MainWindow);
            GlobalCaChe.clipboardHooker.ClipboardUpdated += OnClipboardUpdated;
        }
    }
}

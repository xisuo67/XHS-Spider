using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.IService.XHS;
using XHS.Spider.Services;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 搜索
    /// </summary>
    public partial class SearchViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPageServiceNew _pageServiceNew;
        public SearchViewModel(IServiceProvider serviceProvider, IPageServiceNew pageServiceNew, INavigationService navigationService) {
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
            get => inputCommand ?? (inputCommand = new RelayCommand(ExecuteInput));
            set => inputCommand = value;
        }
        /// <summary>
        /// 处理输入事件
        /// </summary>
        private void ExecuteInput()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                var navigation = _navigationService.GetNavigationControl();
                //TODO:
                SearchService.SearchInput(InputText, navigation, _serviceProvider, _pageServiceNew);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
        }
    }
}

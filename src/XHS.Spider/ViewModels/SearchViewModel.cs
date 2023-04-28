using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using XHS.IService.XHS;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 搜索
    /// </summary>
    public partial class SearchViewModel : ObservableObject, INavigationAware
    {
        private readonly ISearchService _searchService;
        public SearchViewModel(ISearchService searchService) {
            _searchService= searchService;
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
            //TODO:
            _searchService.SearchInput(InputText);
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
        }
    }
}

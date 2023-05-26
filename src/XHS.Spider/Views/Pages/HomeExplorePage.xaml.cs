using Wpf.Ui.Common.Interfaces;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// HomeExplorePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomeExplorePage : INavigableView<HomeExploreViewModel>
    {
        public HomeExploreViewModel ViewModel
        {
            get;
        }
        public HomeExplorePage(HomeExploreViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}

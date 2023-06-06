using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Helpers;
using XHS.IService.XHS;
using XHS.Models.XHS.ApiOutputModel.Search.BusinessModel;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Models.XHS.InputModel;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 搜索首页
    /// </summary>
    public class HomeExploreViewModel : BaseSearchViewModel, INavigationAware
    {
        #region 属性
        private readonly ISnackbarService _snackbarService;
        private readonly IXhsSpiderService _xhsSpiderService;
        private ObservableCollection<SearchNodesModel> _nodes = new ObservableCollection<SearchNodesModel>();

        public ObservableCollection<SearchNodesModel> Nodes
        {
            get { return _nodes; }
            set => SetProperty(ref _nodes, value);
        }
        private int _currentPage = 1;
        private IEnumerable<SearchNode> _searchNodes = new SearchNode[] { };

        public IEnumerable<SearchNode> SearchNodes { 
            get=> _searchNodes;
            set=> SetProperty(ref _searchNodes, value);
        }

        public HomeExploreViewModel(ISnackbarService snackbarService, IXhsSpiderService xhsSpiderService) {
            _snackbarService = snackbarService;
            _xhsSpiderService = xhsSpiderService;
        }
        #endregion

        #region 搜索
        /// <summary>
        /// 处理输入事件
        /// </summary>
        public override async void ExecuteInitData()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                var keyword = HttpUtility.UrlDecode(InputText);
                this.InputText= keyword;
                SearchInputModel model = new SearchInputModel() {
                    keyword = keyword,
                    search_id = AlgorithmHelper.GetSearchId()
                };
                var apiResult = await _xhsSpiderService.SearchNotes(model);
                List<SearchNode> nodes = new List<SearchNode>();
                for (int i = 0; i < 20; i++)
                {
                    Random ran = new Random();
                    int ranNum = ran.Next(1, 5);
                    SearchNode searchNode = new SearchNode()
                    {
                        avatar = "https://sns-avatar-qc.xhscdn.com/avatar/640ee7a7e64abc0b310374b2.jpg?imageView2/2/w/80/format/jpg",
                        CoverUrl = "https://sns-img-qc.xhscdn.com/1000g008271vlc9ifm0005ovens0jq7qrqb6a3bg",
                        display_title = "春天的海边的微风🌊配上爱如火🔥",
                        liked = false,
                        liked_count = "100",
                        nickname = "橙北北111111111111122222222222222222" + i,
                        NodeId = "64102e0d000000000800fa8e",
                        user_id = "63eebf01000000000f011f5b",
                        CoverImage = new BitmapImage(new Uri($"pack://application:,,,/Resources/test{ranNum}.png"))
                    };
                    nodes.Add(searchNode);
                }
                App.PropertyChangeAsync(new Action(() =>
                {
                    this.SearchNodes = nodes.ToArray();
                }));
                SearchNodesModel searchNodesModel = new SearchNodesModel()
                {
                    Page = 1,
                    NodeItems = nodes
                };
                this.Nodes.Add(searchNodesModel);
            }
        }
        #endregion
        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
        }
    }
}

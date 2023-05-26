using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using XHS.Models.XHS.ApiOutputModel.Search.BusinessModel;
using XHS.Models.XHS.ApiOutputModel.UserPosted;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 搜索首页
    /// </summary>
    public class HomeExploreViewModel : BaseSearchViewModel, INavigationAware
    {
        #region 属性
        private string inputText;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }
        private ObservableCollection<SearchNodesModel> _nodes = new ObservableCollection<SearchNodesModel>();

        public ObservableCollection<SearchNodesModel> Nodes
        {
            get { return _nodes; }
            set => SetProperty(ref _nodes, value);
        }

        private IEnumerable<SearchNode> _searchNodes = new SearchNode[] { };

        public IEnumerable<SearchNode> SearchNodes { 
            get=> _searchNodes;
            set=> SetProperty(ref _searchNodes, value);
        }
        // 输入确认事件
        private ICommand inputCommand;
        public ICommand InputCommand
        {
            get => inputCommand ?? (inputCommand = new Wpf.Ui.Common.RelayCommand(ExecuteInitData));
            set => inputCommand = value;
        }
        #endregion

        #region 搜索
        /// <summary>
        /// 处理输入事件
        /// </summary>
        public override async void ExecuteInitData()
        {
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
                    CoverImage= new BitmapImage(new Uri($"pack://application:,,,/Resources/test{ranNum}.png"))
            };
                nodes.Add(searchNode);
            }
            this.SearchNodes = nodes.ToArray();
            SearchNodesModel searchNodesModel = new SearchNodesModel()
            {
                Page = 1,
                NodeItems = nodes
            };
            this.Nodes.Add(searchNodesModel);

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

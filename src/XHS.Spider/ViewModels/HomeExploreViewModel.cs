using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using XHS.Models.XHS.ApiOutputModel.Search.BusinessModel;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 搜索首页
    /// </summary>
    public class HomeExploreViewModel : ObservableObject, INavigationAware
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
        public async void ExecuteInitData()
        {
            //SearchNodesModel searchNodesModel = new SearchNodesModel()
            //    {
            //        Page
            //    };
            List<SearchNode> nodes = new List<SearchNode>();
            for (int i = 0; i < 20; i++)
            {
                SearchNode searchNode = new SearchNode()
                {
                    avatar= "https://sns-avatar-qc.xhscdn.com/avatar/640ee7a7e64abc0b310374b2.jpg?imageView2/2/w/80/format/jpg",
                    coverUrl= "https://sns-img-qc.xhscdn.com/1000g008271vlc9ifm0005ovens0jq7qrqb6a3bg?imageView2",
                    display_title= "春天的海边的微风🌊配上爱如火🔥",
                    liked=false,
                    liked_count=800,
                    nickname= "橙北北"+i,
                    NodeId= "64102e0d000000000800fa8e",
                    user_id= "63eebf01000000000f011f5b",
                };
                nodes.Add(searchNode);
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

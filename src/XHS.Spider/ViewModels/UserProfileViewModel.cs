using CommunityToolkit.Mvvm.ComponentModel;
using Downloader;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using UpdateChecker.Interfaces;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Helpers;
using XHS.Common.Utils;
using XHS.IService.XHS;
using XHS.Models.DownLoad;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Service.Log;
using XHS.Spider.Helpers;
using XHS.Spider.Services;
using XHS.Spider.Views.Windows;

namespace XHS.Spider.ViewModels
{
    public partial class UserProfileViewModel : BaseSearchViewModel, INavigationAware
    {
        private static readonly Service.Log.ILogger Logger = LoggerService.Get(typeof(UserProfileViewModel));
        #region 变量
        public static readonly string BaseUrl = "https://www.xiaohongshu.com/user/profile/";
        public static readonly string BaseVideoUrl = "http://sns-video-bd.xhscdn.com/{0}";
        public static readonly string BaseImageUrl = "https://sns-img-bd.xhscdn.com/{0}?imageView2/format/png";
        //private string inputText;
        //public string InputText
        //{
        //    get => inputText;
        //    set => SetProperty(ref inputText, value);
        //}
        private readonly TaskbarIcon _notifyIcon;
        private string inputSearchText;
        public string InputSearchText
        {
            get => inputSearchText;
            set => SetProperty(ref inputSearchText, value);
        }
        private readonly ISnackbarService _snackbarService;
        private readonly IXhsSpiderService _xhsSpiderService;

        private IEnumerable<NoteModel> _dataGridItemCollection = new NoteModel[] { };
        /// <summary>
        /// 列表显示数据
        /// </summary>
        public IEnumerable<NoteModel> DataGridItemCollection
        {
            get => _dataGridItemCollection;
            set => SetProperty(ref _dataGridItemCollection, value);
        }

        private IEnumerable<NoteModel> _nodes = new NoteModel[] { };
        /// <summary>
        /// 用于操作数据
        /// </summary>
        public IEnumerable<NoteModel> Nodes { get => _nodes; set => SetProperty(ref _nodes, value); }
        /// <summary>
        /// 笔记数量
        /// </summary>
        private string _noteCount;
        public string NoteCount { 
            get=> _noteCount;
            set=>SetProperty(ref _noteCount, value);
        }
        /// <summary>
        /// 解析数量
        /// </summary>
        private string _parseNodeCount = "已解析(0)条";
        public string ParseNodeCount
        {
            get => _parseNodeCount;
            set => SetProperty(ref _parseNodeCount, value);
        }
        private BitmapImage _headImage = new BitmapImage();

        public BitmapImage HeadImage
        {
            get => _headImage;
            set => SetProperty(ref _headImage, value);
        }
        private BitmapImage _sexImage = new BitmapImage();
        public BitmapImage SexImage
        {
            get => _sexImage;
            set => SetProperty(ref _sexImage, value);
        }

        private Tag info = new Tag();
        public Tag Info
        {
            get => info;
            set => SetProperty(ref info, value);
        }

        private Tag location = new Tag();
        public Tag Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }
        private Tag profession = new Tag();

        public Tag Profession
        {
            get => profession; set => SetProperty(ref profession, value);
        }
        /// <summary>
        /// 关注
        /// </summary>
        private Interaction follows = new Interaction();

        public Interaction Follows
        {
            get => follows; set => SetProperty(ref follows, value);
        }

        /// <summary>
        /// 粉丝
        /// </summary>
        private Interaction fans = new Interaction();

        public Interaction Fans
        {
            get => fans; set => SetProperty(ref fans, value);
        }
        /// <summary>
        /// 获赞与收藏
        /// </summary>
        private Interaction interaction = new Interaction();

        public Interaction Interaction
        {
            get => interaction; set => SetProperty(ref interaction, value);
        }
        private OtherInfoModel _userInfo = new OtherInfoModel();
        public OtherInfoModel UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }
        public UserProfileViewModel(ISnackbarService snackbarService, IXhsSpiderService xhsSpiderService)
        {
            _notifyIcon = new TaskbarIcon();
            _snackbarService = snackbarService;
            _xhsSpiderService = xhsSpiderService;
        }

        private ICommand inputNodeSearchCommand;
        public ICommand InputNodeSearchCommand
        {
            get => inputNodeSearchCommand ?? (inputNodeSearchCommand = new Wpf.Ui.Common.RelayCommand(ExecuteSearchNode));
            set => inputNodeSearchCommand = value;
        }
        private ICommand parseNode;

        public ICommand ParseNode
        {
            get => parseNode ?? (parseNode = new CommunityToolkit.Mvvm.Input.RelayCommand(ParseNodeProcess));
            set => parseNode = value;
        }

        private ICommand downLoadCheckAll;

        public ICommand DownLoadCheackAll
        {
            get => downLoadCheckAll ?? (downLoadCheckAll = new CommunityToolkit.Mvvm.Input.RelayCommand(DownLoadCheckAll));
            set => downLoadCheckAll = value;
        }
        private ICommand downLoadAll;

        public ICommand DownLoadAll
        {
            get => downLoadAll ?? (downLoadAll = new CommunityToolkit.Mvvm.Input.RelayCommand(DownLoadAllNodes));
            set => downLoadAll = value;
        }
        #endregion
        #region 下载
        /// <summary>
        /// 下载选中笔记
        /// </summary>
        public void DownLoadCheckAll()
        {
            var cheackNodes = this.Nodes.Where(e => e.IsDownLoad == true);
            if (cheackNodes.Count() > 0)
            {
                var hasNoParseNode = CheckDownLoadNodesData(cheackNodes);
                if (!hasNoParseNode)
                {
                    DownLoad(cheackNodes,false);
                }
            }
            else
            {
                _snackbarService.Show("提示", "请选择下载项", SymbolRegular.ErrorCircle12, ControlAppearance.Danger);
            }
        }
        /// <summary>
        /// 下载所有笔记
        /// </summary>
        public void DownLoadAllNodes()
        {
            var nodes = this.Nodes;
            var hasNoParseNode = CheckDownLoadNodesData(nodes);
            if (!hasNoParseNode)
            {
                DownLoad(nodes);
            }
        }
        /// <summary>
        /// 检查待下载项
        /// </summary>
        /// <param name="downLoadNodes"></param>
        /// <returns></returns>
        private bool CheckDownLoadNodesData(IEnumerable<NoteModel> downLoadNodes)
        {
            bool hasNoParseNode = true;
            var noParseCount = downLoadNodes.Where(e => e.IsParse == false).Count();
            if (noParseCount > 0)
            {
                _snackbarService.Show("提示", "当前待下载任务中存在未解析笔记数据，无法下载", SymbolRegular.ErrorCircle12, ControlAppearance.Danger);
            }
            else
            {
                hasNoParseNode = false;
            }
            return hasNoParseNode;
        }
        /// <summary>
        /// 解析下载资源
        /// </summary>
        private async void ParseNodeProcess()
        {
            _snackbarService.Show("提示", "开始解析", SymbolRegular.ErrorCircle12, ControlAppearance.Success);
            //TODO:有勾选项解析勾选项无勾选项解析所有笔记
            IEnumerable<NoteModel> nodes = null;
            var cheackNodes = this.Nodes.Where(e => e.IsDownLoad == true&& e.IsParse==false);
            if (cheackNodes.Any()) {
                nodes = cheackNodes;
            }
            else
            {
                nodes = this.Nodes.Where(e =>e.IsParse == false);  
            }
            
            string dirName = Format.FormatFileName(UserInfo.BasicInfo.NickName);
            string dirPath = $"{AppDomain.CurrentDomain.BaseDirectory}DownLoad\\{dirName}";
            //循环笔记数据
            foreach (var item in nodes)
            {
                List<DownloadItem> downloadItems = new List<DownloadItem>();
                var resultData = await _xhsSpiderService.GetNodeDetail(item.NoteId);
                if (resultData != null && resultData.Success)
                {
                    var nodeDetail = resultData.Data.Items;
                    foreach (var detailItem in nodeDetail)
                    {
                        var nodeCard = detailItem.NoteCard;
                        var title = Format.FormatFileName(detailItem.NoteCard.Title);
                        var time = FileHelper.TransTime(detailItem.NoteCard.LastUpdateTime);
                        switch (nodeCard.Type)
                        {
                            case "normal":
                                for (int i = 0; i < nodeCard.ImageList.Count; i++)
                                {
                                    var imageUrl = string.Format(BaseImageUrl, nodeCard.ImageList[i].TraceId);
                                    var fpath = $"{dirPath}\\{time}_{title}\\{title}-{Guid.NewGuid().ToString()}.png";
                                    DownloadItem downloadImageItem = new DownloadItem()
                                    {
                                        Url = imageUrl,
                                        FileName = fpath,
                                        Title = title,
                                        FolderPath = $"{dirPath}\\{time}_{title}",
                                        Status = DownloadStatus.None,
                                        FileCount = nodeCard.ImageList.Count,
                                    };
                                    downloadItems.Add(downloadImageItem);
                                }

                                break;
                            case "video":
                                var videoUrl = string.Format(BaseVideoUrl, nodeCard.Video.Consumer.OriginVideoKey);
                                var filePath = $"{dirPath}\\{time}_{title}\\{title}-{Guid.NewGuid().ToString()}.mov";
                                DownloadItem downloadItem = new DownloadItem()
                                {
                                    Url = videoUrl,
                                    FileName = filePath,
                                    Title = title,
                                    FolderPath = $"{dirPath}\\{time}_{title}",
                                    Status = DownloadStatus.None,
                                    FileCount = 1,
                                };
                                downloadItems.Add(downloadItem);
                                break;
                            default:
                                break;
                        }
                    }
                    var nodeEntity = this.Nodes.FirstOrDefault(e => e.NoteId == item.NoteId);
                    if (nodeEntity != null)
                    {
                        nodeEntity.IsParse = true;
                        nodeEntity.FileCount = downloadItems.Count();
                        nodeEntity.DownloadItems= downloadItems;
                        nodeEntity.IsNormal = true;
                    }
                    this.ParseNodeCount = $"已解析({this.Nodes.Where(e => e.IsParse == true).Count()})条";
                }
                else
                {
                    //网络连接异常，请检查网络设置或重启试试
                    if (resultData.Code== 300012)
                    {
                        _notifyIcon.ShowBalloonTip($"解析失败，接口异常。已完成解析【{this.Nodes.Where(e => e.IsParse == true).Count()}】条笔记", "提示", BalloonIcon.Error);
                        break;
                    }
                    else
                    {
                        //-510001  笔记状态异常，请稍后查看
                        if (resultData.Code == -510001)
                        { 
                            //TODO:其他异常，将笔记状态改为异常
                            var nodeEntity = this.Nodes.FirstOrDefault(e => e.NoteId == item.NoteId);
                            if (nodeEntity != null)
                            {
                                nodeEntity.IsParse = true;
                                nodeEntity.FileCount = 0;
                                nodeEntity.DownloadItems = downloadItems;
                                nodeEntity.IsNormal = false;
                            }
                            this.ParseNodeCount = $"已解析({this.Nodes.Where(e => e.IsParse == true).Count()})条";
                        }
                    }
                  
                }
                Random ran = new Random();
                int awaitTime = ran.Next(2800, 3500);
                await Task.Delay(awaitTime);
                this.DataGridItemCollection = this.Nodes.ToArray();
            }
            var parseNodes= nodes.Where(e =>e.IsParse == false);
            if (parseNodes.Count()==0)
            {
                _notifyIcon.ShowBalloonTip($"解析完成", "提示", BalloonIcon.Info);
            }
        }

        /// <summary>
        /// 下载笔记
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="isDownLoadAll"></param>
        private void DownLoad(IEnumerable<NoteModel> nodes, bool isDownLoadAll = true)
        {
            List<DownloadItem> downloadItems = new List<DownloadItem>();
            foreach (var node in nodes)
            {
                if (node.DownloadItems.Count > 0)
                {
                    downloadItems.AddRange(node.DownloadItems);
                }
            }
            App.DownloadList.AddRange(downloadItems);
            if (isDownLoadAll)
            {
                _snackbarService.Show("提示", "开始下载所有笔记", SymbolRegular.Checkmark12, ControlAppearance.Success);
            }
            else
            {
                _snackbarService.Show("提示", "开始下载选中笔记", SymbolRegular.Checkmark12, ControlAppearance.Success);
            }
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
                if (InputText.Contains("user/profile/"))
                {
                    var id = SearchService.GetId(InputText, BaseUrl);
                    if (string.IsNullOrEmpty(id))
                    {
                        Logger.Error($"URL有误：{InputText}");
                        InitNullImage();
                        return;
                    }
                    else
                    {
                        this.ParseNodeCount = "已解析(0)条";
                        var apiResult = await _xhsSpiderService.GetOtherInfo(id);
                        if (apiResult != null && apiResult.Success)
                        {
                            UserInfo = apiResult.Data;
                            var info = UserInfo.Tags.FirstOrDefault(e => e.TagType == "info");
                            if (info != null)
                                Info = info;
                            var location = UserInfo.Tags.FirstOrDefault(e => e.TagType == "location");
                            if (location != null)
                                Location = location;
                            var profession = UserInfo.Tags.FirstOrDefault(e => e.TagType == "profession");
                            if (profession != null)
                                Profession = profession;
                            var follows = UserInfo.Interactions.FirstOrDefault(e => e.Type == "follows");
                            if (follows != null)
                                Follows = follows;
                            var fans = UserInfo.Interactions.FirstOrDefault(e => e.Type == "fans");
                            if (fans != null)
                                Fans = fans;
                            var interaction = UserInfo.Interactions.FirstOrDefault(e => e.Type == "interaction");
                            if (interaction != null)
                                Interaction = interaction;

                            App.PropertyChangeAsync(new Action(() =>
                            {
                                var baseInfo = UserInfo.BasicInfo;
                                if (baseInfo != null && !string.IsNullOrEmpty(baseInfo.Imageb))
                                {
                                    //TODO:处理url？号后参数
                                    var imageUrl = baseInfo.Imageb.Split('?')[0];
                                    var headImage = FileHelper.UrlToBitmapImage(imageUrl);
                                    HeadImage = headImage;
                                }
                                var filePath = AppDomain.CurrentDomain.BaseDirectory + "Resources";
                                if (!string.IsNullOrEmpty(info?.Icon))
                                {
                                    BitmapImage sex = null;
                                    if (info?.Icon.Contains("gender-female-v1.png") == true)
                                    {
                                        sex = new BitmapImage(new Uri("pack://application:,,,/Resources/gender-female-v1.png"));
                                       
                                    }
                                    else if (info?.Icon.Contains("gender-male-v1.png") == true)
                                    {
                                        sex = new BitmapImage(new Uri("pack://application:,,,/Resources/gender-male-v1.png"));
                                    }
                                    else
                                    {
                                        sex = FileHelper.UrlToBitmapImage(info.Icon);
                                    }
                                    SexImage = sex;
                                }
                                else
                                {
                                    SexImage = null;
                                }
                            }));

                            var nodes =await _xhsSpiderService.GetAllUserNode(id);
                            foreach (var node in nodes)
                            {
                                node.LikedCount = node.interact_info?.LikedCount;
                            }
                            Nodes = nodes.ToArray();
                            NoteCount =$"({nodes.Count()})条" ;
                            DataGridItemCollection = Nodes;
                        }
                        else
                        {
                            InitNullImage();
                            _snackbarService.Show("异常", apiResult?.Msg, SymbolRegular.ErrorCircle12, ControlAppearance.Danger);
                        }
                    }
                }
                else
                {
                    _snackbarService.Show("提示", "当前Url不符合所属模块搜索要求", SymbolRegular.ErrorCircle12, ControlAppearance.Danger);
                }
            }
        }
        public void InitNullImage()
        {
            App.PropertyChangeAsync(new Action(() =>
            {
                HeadImage = null;
                SexImage = null;
            }));
        }
        /// <summary>
        /// 搜索笔记
        /// </summary>
        public void ExecuteSearchNode()
        {
            var searchText = this.InputSearchText;
            if (!string.IsNullOrEmpty(searchText))
            {
                var searchNodes = this.Nodes.Where(e => e.DisplayTitle.Contains(searchText));
                this.DataGridItemCollection = searchNodes;
            }
            else
            {
                this.DataGridItemCollection = this.Nodes;
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Downloader;
using Microsoft.ClearScript;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Utils;
using XHS.IService.XHS;
using XHS.Models.DownLoad;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Spider.Helpers;
using XHS.Spider.Services;

namespace XHS.Spider.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject, INavigationAware
    {
        #region 变量
        public static readonly string BaseUrl = "https://www.xiaohongshu.com/user/profile/";
        public static readonly string BaseVideoUrl = "http://sns-video-bd.xhscdn.com/{0}";
        public static readonly string BaseImageUrl = "https://sns-img-bd.xhscdn.com/{0}?imageView2/format/png";
        private static ConcurrentDictionary<string, string> _downLoadDic = new ConcurrentDictionary<string, string>();
        private string inputText;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }
        private readonly ISnackbarService _snackbarService;
        private readonly IXhsSpiderService _xhsSpiderService;

        private IEnumerable<NoteModel> _nodes = new NoteModel[] { };

        public IEnumerable<NoteModel> Nodes { get => _nodes; set => SetProperty(ref _nodes, value); }
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
            _snackbarService = snackbarService;
            _xhsSpiderService = xhsSpiderService;
        }


        // 输入确认事件
        private ICommand inputCommand;
        public ICommand InputCommand
        {
            get => inputCommand ?? (inputCommand = new Wpf.Ui.Common.RelayCommand(ExecuteInitData));
            set => inputCommand = value;
        }

        private ICommand downLoadAll;

        public ICommand DownLoadAll
        {
            get => downLoadAll ?? (downLoadAll = new CommunityToolkit.Mvvm.Input.RelayCommand(DownLoadAllNodes));
            set => downLoadAll = value;
        }
        #endregion

        public void DownLoadAllNodes()
        {
            var nodes = this.Nodes;
            _downLoadDic.Clear();
            string dirName = Format.FormatFileName(UserInfo.BasicInfo.NickName);
            string dirPath = $"{AppDomain.CurrentDomain.BaseDirectory}DownLoad\\{dirName}";
            List<DownloadItem> downloadItems = new List<DownloadItem>();
            //循环笔记数据
            foreach (var item in nodes)
            {
                var resultData = _xhsSpiderService.GetNodeDetail(item.NoteId);
                if (resultData != null && resultData.Success)
                {
                    var nodeDetail = resultData.Data.Items;
                    foreach (var detailItem in nodeDetail)
                    {
                        var nodeCard = detailItem.NoteCard;
                        var title = Format.FormatFileName(detailItem.NoteCard.Title);
                        switch (nodeCard.Type)
                        {
                            case "normal":
                                for (int i = 0; i < nodeCard.ImageList.Count; i++)
                                {
                                    var imageUrl = string.Format(BaseImageUrl, nodeCard.ImageList[i].TraceId);
                                    var fpath = $"{dirPath}\\{title}\\{title}-{Guid.NewGuid().ToString()}.png";
                                    DownloadItem downloadImageItem = new DownloadItem() { 
                                        Url= imageUrl,
                                        FileName = fpath,
                                        Title= title,
                                        FolderPath= $"{dirPath}\\{title}",
                                        Status = DownloadStatus.None,
                                        FileCount=nodeCard.ImageList.Count,
                                    };
                                    downloadItems.Add(downloadImageItem);
                                }

                                break;
                            case "video":
                                var videoUrl = string.Format(BaseVideoUrl, nodeCard.Video.Consumer.OriginVideoKey);
                                var filePath = $"{dirPath}\\{title}\\{title}-{Guid.NewGuid().ToString()}.mov";
                                DownloadItem downloadItem = new DownloadItem()
                                {
                                    Url = videoUrl,
                                    FileName = filePath,
                                    Title = title,
                                    FolderPath= $"{dirPath}\\{title}",
                                    Status = DownloadStatus.None,
                                    FileCount=1,
                                };
                                downloadItems.Add(downloadItem);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            App.DownloadList.AddRange(downloadItems);

            _snackbarService.Show("提示", "开始下载所有笔记", SymbolRegular.Checkmark12, ControlAppearance.Success);
        }

        /// <summary>
        /// 处理输入事件
        /// </summary>
        public void ExecuteInitData()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (InputText.Contains("user/profile/"))
                {
                    var id = SearchService.GetId(inputText, BaseUrl);
                    if (string.IsNullOrEmpty(id))
                    {
                        return;
                    }
                    else
                    {
                        var apiResult = _xhsSpiderService.GetOtherInfo(id);
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
                                if (!string.IsNullOrEmpty(info?.Icon))
                                {
                                    var sex = FileHelper.UrlToBitmapImage(info.Icon);
                                    SexImage = sex;
                                }
                            }));
                            
                            var nodes = _xhsSpiderService.GetAllUserNode(id);
                            foreach (var node in nodes)
                            {
                                node.LikedCount = node.interact_info?.LikedCount;
                            }
                            Nodes = nodes.ToArray();
                        }
                        else
                        {
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

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {

        }
    }
}

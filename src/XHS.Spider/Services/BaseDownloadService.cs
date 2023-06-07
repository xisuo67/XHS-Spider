using Downloader;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UpdateChecker.Interfaces;
using XHS.Common.Utils;
using XHS.Models.DownLoad;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;
using XHS.Service.Log;
using XHS.Spider.Helpers;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Services
{
    public class BaseDownloadService
    {
        private static readonly Service.Log.ILogger Logger = LoggerService.Get(typeof(BaseDownloadService));
        private string CurrentFolderPath;
        private  TaskbarIcon _notifyIcon;
        protected Task workTask;
        protected ObservableCollection<DownloadItem> _downloadList;
        private DownloadService CurrentDownloadService;
        private DownloadConfiguration CurrentDownloadConfiguration;
        public BaseDownloadService(ObservableCollection<DownloadItem> downloadList)
        {
            this._downloadList = downloadList;
        }

        /// <summary>
        /// 启动下载服务
        /// </summary>
        public void Start()
        {
            _notifyIcon = new TaskbarIcon();
            _notifyIcon.TrayBalloonTipClicked += notifyIcon_TrayBalloonTipClicked;
            workTask = Task.Run(DoWork);
        }
        public async Task DoWork()
        {
            while (true)
            {
                var downLoadList = _downloadList.Where(e => e.Status == DownloadStatus.None).ToList();
                if (downLoadList.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < downLoadList.Count; i++)
                        {
                            var downloadItem = downLoadList[i];
                            downloadItem.Status = DownloadStatus.Running;
                            await DownloadFile(downloadItem).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("下载服务异常",ex);
                    }
                }
                else { 
                    var downLoaded= _downloadList.Where(e => e.Status == DownloadStatus.Completed).ToList();
                    foreach (var item in downLoaded)
                    {
                        _downloadList.Remove(item);
                    }
                }
                // 降低CPU占用
                await Task.Delay(500);
            }
        }
        private async Task<DownloadService> DownloadFile(DownloadItem downloadItem)
        {
            CurrentDownloadConfiguration = GetDownloadConfiguration();
            CurrentDownloadService = CreateDownloadService(CurrentDownloadConfiguration);

            if (string.IsNullOrWhiteSpace(downloadItem.FileName))
            {
                await CurrentDownloadService.DownloadFileTaskAsync(downloadItem.Url, new DirectoryInfo(downloadItem.FolderPath)).ConfigureAwait(false);
            }
            else
            {
                await CurrentDownloadService.DownloadFileTaskAsync(downloadItem.Url, downloadItem.FileName).ConfigureAwait(false);
            }

            return CurrentDownloadService;
        }
        private DownloadConfiguration GetDownloadConfiguration()
        {
            return new DownloadConfiguration
            {
                BufferBlockSize = 10240, // 通常，主机最大支持8000字节，默认值为8000。
                ChunkCount = 1, // 要下载的文件分片数量，默认值为1
                MaxTryAgainOnFailover = 3, // 失败的最大次数
                ParallelDownload = true, // 下载文件是否为并行的。默认值为false
                Timeout = 1000, // 每个 stream reader  的超时（毫秒），默认值是1000
            };
        }
        private DownloadService CreateDownloadService(DownloadConfiguration config)
        {
            var downloadService = new DownloadService(config);
            // Download completed event that can include occurred errors or 
            // cancelled or download completed successfully.
            downloadService.DownloadFileCompleted += OnDownloadFileCompleted;

            return downloadService;
        }
        private void notifyIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CurrentFolderPath))
            {
                Utils.OpenURL(CurrentFolderPath);
            }
        }
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                //异步操作已取消
            }
            else if (e.Error != null)
            {
                //异常

            }
            else
            {
                //下载完成
                var downLoadInfo = e.UserState as DownloadPackage;
                App.PropertyChangeAsync(new Action(() =>
                {
                    var entity = _downloadList.FirstOrDefault(x => x.FileName == downLoadInfo.FileName);
                    if (entity != null)
                    {
                        entity.Status = DownloadStatus.Completed;
                        //TODO:搜索文件夹路径文件，判断是否与文件数量一致;
                        System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(entity.FolderPath);
                        int fileCount = Utils.GetFilesCount(dirInfo);
                        if (fileCount == entity.FileCount)
                        {
                            CurrentFolderPath=entity.FolderPath;
                            _notifyIcon.ShowBalloonTip("下载完成", $"【{entity.Title}】\n点击查看下载文件\n剩余【{_downloadList.Count()-1}】文件待下载", BalloonIcon.Info);
                        }
                        //加入到下载完成list中，并从下载中list去除
                        _downloadList.Remove(entity);
                    }
                }));
            }
        }
    }
}

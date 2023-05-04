using Downloader;
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
using XHS.Models.DownLoad;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;

namespace XHS.Spider.Services
{
    public class BaseDownloadService
    {
        protected Task workTask;
        protected ObservableCollection<DownloadItem> _downloadList;
        private DownloadService CurrentDownloadService;
        private DownloadConfiguration CurrentDownloadConfiguration;
        protected List<Task> downloadingTasks = new List<Task>();
        public BaseDownloadService(ObservableCollection<DownloadItem> downloadList) { 
            this._downloadList = downloadList;
        }

        /// <summary>
        /// 启动下载服务
        /// </summary>
        public void Start()
        {
            workTask = Task.Run(DoWork);
        }
        public async Task DoWork() { 
            while (true)
            {
                if (downloadingTasks.Count>0)
                {
                    downloadingTasks.RemoveAll((m) => m.IsCompleted);
                }

                if (_downloadList.Count>0)
                {
                    try
                    {
                        for (int i = 0; i < _downloadList.Count; i++)
                        {
                            await DownloadFile(_downloadList[i]).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
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
        private  DownloadConfiguration GetDownloadConfiguration()
        {
            return new DownloadConfiguration
            {
                BufferBlockSize = 10240,    // usually, hosts support max to 8000 bytes, default values is 8000
                ChunkCount = 12,             // file parts to download, default value is 1
                MaximumBytesPerSecond = 1024 * 1024 * 10, // download speed limited to 10MB/s, default values is zero or unlimited
                MaxTryAgainOnFailover = 3,  // the maximum number of times to fail
                ParallelDownload = false,    // download parts of file as parallel or not. Default value is false
                ParallelCount = 12,          // number of parallel downloads. The default value is the same as the chunk count
                Timeout = 3000,             // timeout (millisecond) per stream block reader, default value is 1000
                ClearPackageOnCompletionWithFailure = true, // Clear package and downloaded data when download completed with failure, default value is false
                MinimumSizeOfChunking = 1024, // minimum size of chunking to download a file in multiple parts, default value is 512        
                ReserveStorageSpaceBeforeStartingDownload = true, // Before starting the download, reserve the storage space of the file as file size, default value is false
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
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
           
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
                        //加入到下载完成list中，并从下载中list去除
                        _downloadList.Remove(entity);
                    }
                }));
            }
        }
    }
}

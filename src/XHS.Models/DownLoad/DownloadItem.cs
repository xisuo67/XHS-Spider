using Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.DownLoad
{
    public class DownloadItem
    {
        /// <summary>
        /// 文件夹地址
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// 笔记标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url { get; set; }
        public DownloadStatus Status { get; set; }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }
    }
}

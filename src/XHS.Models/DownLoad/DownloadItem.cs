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
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public DownloadStatus Status { get; set; }
    }
}

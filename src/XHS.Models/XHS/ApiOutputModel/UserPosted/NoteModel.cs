using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.DownLoad;
using XHS.Models.Enum;

namespace XHS.Models.XHS.ApiOutputModel.UserPosted
{
    /// <summary>
    /// 笔记信息
    /// </summary>
    public class NoteModel
    {
        /// <summary>
        /// 用于区分返回实体笔记类型
        /// </summary>
        public NoteTypeEnum NoteTypeEnum { get; set; }
        private string type = string.Empty;
        [JsonProperty("type")]

        public string Type
        {
            get => type;
            set
            {
                type = value;
                switch (type)
                {
                    case "normal":
                        NodeType = "笔记";
                        break;
                    case "video":
                        NodeType = "视频";
                        break;
                    default:
                        break;
                }
            }
        }

        [JsonProperty("display_title")]
        public string DisplayTitle { get; set; }

        [JsonProperty("user")]
        public NodeUser User { get; set; }

        [JsonProperty("interact_info")]
        public NodeInteractInfo interact_info { get; set; }

        [JsonProperty("cover")]
        public NodeCover cover { get; set; }

        [JsonProperty("note_id")]
        public string NoteId { get; set; }
        /// <summary>
        /// 文件数量
        /// </summary>
        public int? FileCount { get; set; }
        private bool isHidden = false;
        /// <summary>
        /// 是否解析
        /// </summary>
        public bool IsParse
        {
            get => isHidden;
            set
            {
                isHidden = value;
                this.IsParseStr = isHidden ? "是" : "否";
            }
        }

        private bool isNormal = true;

        public bool IsNormal { 
            get=> isNormal;
            set {

                isNormal = value;
                this.Status = IsNormal ? "正常" : "异常";
             }
        }
        /// <summary>
        /// 笔记状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 是否解析中文字段
        /// </summary>
        public string IsParseStr { get; set; }
        /// <summary>
        /// 笔记喜欢人数
        /// </summary>
        public string LikedCount { get; set; }

        /// <summary>
        /// 笔记类型
        /// </summary>
        public string NodeType { get; set; }

        public bool IsDownLoad { get; set; }

        public List<DownloadItem> DownloadItems { get; set; }
    }
}

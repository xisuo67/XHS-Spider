using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.UserPosted
{
    /// <summary>
    /// 笔记信息
    /// </summary>
    public class NoteModel
    {
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

        public string LikedCount { get; set; }

        public string NodeType { get; set; }
    }
}

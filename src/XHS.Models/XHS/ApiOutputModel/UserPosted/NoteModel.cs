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
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("display_title")]
        public string DisplayTitle { get; set; }

        [JsonProperty("user")]
        public NodeUser User { get; set; }
    }
}

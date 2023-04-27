using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.Common;

namespace XHS.Models.XHS.ApiOutputModel.NodeDetail
{
    public class InteractInfoModel : BaseInteractInfo
    {
        [JsonProperty("followed")]
        public bool Followed { get; set; }

        [JsonProperty("collected")]
        public bool Collected { get; set; }
        [JsonProperty("collected_count")]
        public string CollectedCount { get; set; }
        [JsonProperty("comment_count")]
        public string CommentCount { get; set; }
        [JsonProperty("share_count")]
        public string ShareCount { get; set; }
    }
}

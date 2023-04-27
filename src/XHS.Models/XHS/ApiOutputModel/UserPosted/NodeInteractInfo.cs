using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.UserPosted
{
    public class NodeInteractInfo
    {
        [JsonProperty("followed")]
        public bool Followed { get; set; }
        [JsonProperty("liked")]
        public bool Liked { get; set; }
        [JsonProperty("liked_count")]
        public string LikedCount { get; set; }
        [JsonProperty("collected")]
        public bool Collected { get; set;}
        [JsonProperty("collected_count")]
        public string CollectedCount { get; set; }
        [JsonProperty("comment_count")]
        public string CommentCount { get; set; }
        [JsonProperty("share_count")]
        public string ShareCount { get; set; }
    }
}

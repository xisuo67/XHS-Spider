using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Common
{
    public class BaseInteractInfo
    {
        [JsonProperty("liked")]
        public bool Liked { get; set; }
        [JsonProperty("liked_count")]
        public string LikedCount { get; set; }
    }
}

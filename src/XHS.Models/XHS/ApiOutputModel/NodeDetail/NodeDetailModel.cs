using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.NodeDetail
{
    public class NodeDetailModel
    {
        [JsonProperty("cursor_score")]
        public string CursorScore { get; set; }
        [JsonProperty("items")]
        public NodeItem Items { get; set; }
        [JsonProperty("current_time")]
        public long CurrentTime { get; set; }
    }
}

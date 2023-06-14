using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.Common;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;

namespace XHS.Models.XHS.ApiOutputModel.Search
{
    /// <summary>
    /// 关键字搜索返回笔记数据
    /// </summary>
    public class SearchNodesOutPutModel
    {
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("items")]
        public List<SearchNodeItem> Items { get; set; }
    }
}

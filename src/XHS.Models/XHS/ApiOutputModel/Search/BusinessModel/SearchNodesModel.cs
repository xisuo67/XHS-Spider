using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Search.BusinessModel
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class SearchNodesModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 搜索数据
        /// </summary>
        public List<SearchNodeItem> Items { get; set; }
        /// <summary>
        /// 重新封装后的笔记数据用于前台展示
        /// </summary>
        public List<SearchNode> NodeItems { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Search.BusinessModel
{
    /// <summary>
    /// 业务数据搜索笔记-用于前台展示
    /// </summary>
    public class SearchNode
    {
        /// <summary>
        /// 笔记id
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// 是否喜欢
        /// </summary>
        public bool liked { get; set; }
        /// <summary>
        /// 喜欢数量
        /// </summary>
        public int liked_count { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 用户封面
        /// </summary>
        public string coverUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string display_title { get; set; }
    }
}

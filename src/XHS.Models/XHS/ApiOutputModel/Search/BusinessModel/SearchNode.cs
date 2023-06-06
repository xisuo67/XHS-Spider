
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        public string liked_count { get; set; }
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

        private string coverUrl = string.Empty;
        /// <summary>
        /// 封面
        /// </summary>
        public string CoverUrl { 
            get=>coverUrl; 
            set {
                coverUrl = value;
                this.CoverFullUrl = $"{coverUrl}?imageView2/2/w/640/format/webp|imageMogr2/strip";
            }
        }

        public string CoverFullUrl { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public BitmapImage CoverImage { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string display_title { get; set; }

        public SearchNode Clone()
        {
            return  (SearchNode)this.MemberwiseClone();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.InputModel
{
    public class SearchInputModel
    {
        public string keyword { get; set; }
        /// <summary>
        /// 笔记类型 0:全部，1：视频，2：笔记
        /// </summary>
        public int note_type { get; set; } = 0;
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; } = 1;
        /// <summary>
        /// 条数
        /// </summary>
        public int page_size { get; set; } = 20;

        public string search_id { get; set; }
        /// <summary>
        /// 排序字段：默认：general，最新：time_descending，最热：popularity_descending，
        /// </summary>
        public string sort { get; set; } = "general";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.SettingCookie
{
    public class CookieModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Cookie值
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 是否自动获取
        /// </summary>
        public bool IsGetAutomatically { get; set; } = false;
    }
}

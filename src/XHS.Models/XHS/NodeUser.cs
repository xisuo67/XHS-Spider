using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS
{
    //"user": {
        //"nick_name": "线下不知名小雪",
        //"avatar": "https://sns-avatar-qc.xhscdn.com/avatar/6428b94d888bee6dcaff03d9.jpg",
        //"user_id": "5ad6209c4eacab0c13a61dd4",
        //"nickname": "线下不知名小雪"
    //            },

    /// <summary>
    /// 笔记中用户信息
    /// </summary>
    public class NodeUser
    {
        [JsonProperty("nick_name")]
        public string NickName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// ????返回两个nick_name?????奇葩
        /// </summary>
        [JsonProperty("nickname")]
        public string NickNames { get; set; }
    }
}

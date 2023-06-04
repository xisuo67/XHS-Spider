using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XHS.Model
{
    /// <summary>
    /// 小红书接口返回基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XHSBaseApiModel<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}

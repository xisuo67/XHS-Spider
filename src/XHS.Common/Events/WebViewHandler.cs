using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Events
{
    public class WebViewHandler
    {
        //public static event GetXsXtEventEventHandler GetXsXtEventEvent;
        //public delegate Task<string> GetXsXtEventEventHandler(object sender, string url);


        public static event EventHandler<string> OnSubscribeGetXsXt;//订阅聊天消息
        public delegate Task<string> EventHandler(object sender, string url);
        public static void GetXsXt(string url)
        {
            if (OnSubscribeGetXsXt != null)
            {
                OnSubscribeGetXsXt(null,url);
            }
        }
    }
}

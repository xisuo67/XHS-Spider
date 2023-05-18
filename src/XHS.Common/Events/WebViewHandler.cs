using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Events
{
    public class WebViewHandler
    {
        public static event EventHandler OnSubscribeGetXsXtHandler;//订阅聊天消息
        public delegate Task<string> EventHandler(object sender, string url);
        public static async Task<string> GetXsXt(string url)
        {
            if (OnSubscribeGetXsXtHandler != null)
            {
              return await OnSubscribeGetXsXtHandler.Invoke(null,url);
            }
            else
            {
                return null;
            }
        }
    }
}

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Interfaces;
using XHS.Common.Events;
using XHS.Common.Events.Model;
using XHS.Common.Global;
using XHS.Models.Events;
using XHS.Service.Log;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Helpers
{
    /// <summary>
    /// 网页调用C#方法
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class ScriptHost
    {
        private static IEventAggregator _aggregator { get; set; }
        private static readonly Service.Log.ILogger Logger = LoggerService.Get(typeof(ScriptHost));
        public static event EventHandler OnPublishHandler;//发布页面加载完成时间
        private WebView2 webView;
        public static ScriptHost scriptHost = null;
        private ScriptHost(WebView2 webView)
        {
            scriptHost = this;
            this.webView = webView;
            //注册事件侦听，加载页面完成时，获取cookie；
            this.webView.NavigationStarting += WebView_NavigationStarting;
            this.webView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webView_NavigationCompleted);
            //webView初始化完成后注册与JavaScript交互
            this.webView.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.webView_CoreWebView2InitializationCompleted);
        }
        /// <summary>
        /// 开始跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void WebView_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            //var cookieEntity= GlobalCaChe.Cookies.FirstOrDefault(e=>e.IsGetAutomatically==false);
            string cookie = @"a1=186de24f322i6k85k3pozwymdum8ecsla4a6dcrgr50000322820; webId=50eb4725069e5d0fdb3b38d7dec18649; gid=yYKfdJ4iWj0JyYKfdJ4iq4SFJJ3KTY2TqUlu0kFEv6f76K28YdSM1f888qJJYJ88fWWyjKfj; gid.sign=/z6MNVFYsIl96uA3VEvQwOGynVw=; customerClientId=296074035300859; xhsTrackerId=109c7c25-a067-459e-9bd1-87a12db48ea1; xhsTrackerId.sig=uRtS9bxaCwD1D5U0bysUBIzu3l0ZoRNINCSUR0LwG_w; x-user-id-creator.xiaohongshu.com=643537d4cc5ead00017f3ee1; access-token-creator.xiaohongshu.com=customer.ares.AT-303eb29d32e6473288eb4676bb1be053-fc07a9b202604dcaaa0f3e30f0250dbd; timestamp2=16835252284415e59bfaf1cd7965f6187e0f0453168cbdcce9cd3436f00866b; timestamp2.sig=lAGOQpNSPW0NeeO0HdNzHa9z_Jm77Mm2kWU4snV6Zbg; smidV2=202305121323401a1123c08aef73436afa1d24784bc362004259fa4cec0ee40; xsecappid=xhs-pc-web; web_session=030037a33cbe715cc290d19225234a17a6cec0; webBuild=2.6.6; websectiga=7750c37de43b7be9de8ed9ff8ea0e576519e8cd2157322eb972ecb429a7735d4; sec_poison_id=451ccc43-536d-4f78-8908-c1c1ae9a2aff; extra_exp_ids=yamcha_0327_clt,h5_1208_exp3,ques_clt2; extra_exp_ids.sig=-9P_FIY9nRpp4czlpi3JlPCL_zdr5ZMYd73Vy8sdzzY";
            //处理手动设置的cookie
            var a=cookie.Split(';');
            foreach (var item in a)
            {
                var cookieItem=item.Trim();
                //二次拆分，求name,value
                cookieItem.Split('=');
            }
            //如果存在手动设置cookie，那么更新cookie
            //if (cookieEntity != null)
            //{
            //    var webView2 = sender as WebView2;
            //    if (webView2 != null)
            //    {
            //        var url = webView2.CoreWebView2.Source;
            //        if (url.Contains("https://www.xiaohongshu.com/"))
            //        {
            //            List<CoreWebView2Cookie> cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(url);

            //            CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookie("CookieName", "CookieValue", ".bing.com", "/");
            //            webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
            //            //webView.CoreWebView2.CookieManager.DeleteAllCookies();

            //        }
            //    }
               
            //}
        }

        public static ScriptHost GetScriptHost(WebView2 webView, IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            if (scriptHost == null) scriptHost = new ScriptHost(webView);
            return scriptHost;
        }
        /// <summary>
        /// 日志记录（JavaScript前端调用
        /// </summary>
        /// <param name="message">JavaScript前端信息</param>
        public void log(string message)
        {
            Logger.Debug(message);
        }
        /// <summary>
        /// WebView2异步获取cookie
        /// </summary>
        /// <param name="url">与cookie关联的域名</param>
        private async void GetCookie(string url)
        {
            List<CoreWebView2Cookie> cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(url);
            string cookies =string.Empty;
            for (int i = 0; i < cookieList.Count; ++i)
            {
                CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList[i].ToSystemNetCookie());
                cookies += cookie.Name + "=" + cookie.Value + ";";
            }
            GlobalCaChe.Cookies.Clear();
            GlobalCaChe.Cookies.Add(new XHS.Models.SettingCookie.CookieModel() {
                Id=Guid.NewGuid(),
                Cookie=cookies
            });
        }
        /// <summary>
        /// 加载完页面时，获取cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //TODO:自动获取cookie
            string hosturl = webView.Source.Host.ToString();
            hosturl = "https://" + hosturl;
            GetCookie(hosturl);
            var webView2 = sender as Microsoft.Web.WebView2.Wpf.WebView2;
            if (webView2 != null)
            {
                var url = webView2.CoreWebView2.Source;
                //TODO:发布页面加载完成事件
                RedirectInfo redirectInfo = new RedirectInfo() { Url = url };
                _aggregator.GetEvent<NavigationCompletedEvent>().Publish(redirectInfo);
            }
        }
        /// <summary>
        /// CoreWebView2初始化完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void webView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            //注册winning，winasync脚本c#互操作
            webView.CoreWebView2.AddHostObjectToScript("scriptHost", scriptHost);
            //注册全局变量winning 同步操作；winasync:异步操作；
            webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("var winasync= window.chrome.webview.hostObjects.scriptHost;");
            webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("var winning= window.chrome.webview.hostObjects.sync.scriptHost;");
        }
    }
}

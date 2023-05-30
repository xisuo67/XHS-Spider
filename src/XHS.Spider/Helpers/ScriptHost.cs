using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using XHS.Common.Events;
using XHS.Common.Events.Model;
using XHS.Common.Global;
using XHS.Models.Events;
using XHS.Service.Log;

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
            this.webView.WebMessageReceived += WebView_WebMessageReceived;
            this.webView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webView_NavigationCompleted);
            //webView初始化完成后注册与JavaScript交互
            this.webView.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.webView_CoreWebView2InitializationCompleted);
        }

        private void WebView_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var webView2 = sender as Microsoft.Web.WebView2.Wpf.WebView2;
            if (webView2 != null)
            {
                var url = webView2.CoreWebView2.Source;
               
            }
        }

        public static ScriptHost GetScriptHost(WebView2 webView, IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            if (scriptHost == null) scriptHost = new ScriptHost(webView);
            return scriptHost;
        }

        public static ScriptHost UpdateScriptHost(WebView2 webView, IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            scriptHost = new ScriptHost(webView);
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
            var cookieEntity = GlobalCaChe.Cookies.FirstOrDefault(e => e.IsGetAutomatically == false);
            //如果存在手动设置cookie，那么更新cookie
            if (cookieEntity != null)
            {
                //List<CoreWebView2Cookie> cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(url);
                //处理手动设置的cookie
                var cookis = cookieEntity.Cookie.Split(';');
                foreach (var item in cookis)
                {
                    var cookieItem = item.Trim();
                    //二次拆分，求name,value
                    var cookieItemValue = cookieItem.Split('=');
                    if (cookieItemValue.Count() == 2)
                    {
                        CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookie(cookieItemValue[0], cookieItemValue[1], ".xiaohongshu.com", "/");
                        webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
                    }
                }
            }
            List<CoreWebView2Cookie> cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(url);
            string cookies = string.Empty;
            for (int i = 0; i < cookieList.Count; ++i)
            {
                CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList[i].ToSystemNetCookie());
                cookies += cookie.Name + "=" + cookie.Value + ";";
            }
            GlobalCaChe.Cookies.Clear();
            GlobalCaChe.Cookies.Add(new XHS.Models.SettingCookie.CookieModel()
            {
                Id = Guid.NewGuid(),
                Cookie = cookies,
                IsGetAutomatically=true,
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
            var webView2 = sender as WebView2;
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

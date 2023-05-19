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
        private Dictionary<string, string> mCookies = new Dictionary<string, string>();//保存Cookie到字典中
        public static ScriptHost scriptHost = null;
        private ScriptHost(WebView2 webView)
        {
            scriptHost = this;
            this.webView = webView;
            //订阅事件
            _aggregator.GetEvent<CalcXsXtEvent>().Subscribe(Getxs);
            //注册事件侦听，加载页面完成时，获取cookie；
            this.webView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webView_NavigationCompleted);
            //webView初始化完成后注册与JavaScript交互
            this.webView.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.webView_CoreWebView2InitializationCompleted);
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

        private async void Getxs(string url)
        {
            string jscode = "var url='" + url + "';\r\n" + @"try {
                                                                            sign(url);
                                                                        } catch (e) { winning.log(e); }
                                                                        function sign(url) {
                                                                            var t;
                                                                            var o = window._webmsxyw(url, t);
                                                                            return o;
                                                                        }";
            var xsxtStr = await webView.CoreWebView2.ExecuteScriptAsync(jscode);

            if (!string.IsNullOrEmpty(xsxtStr))
            {
                //JObject xsxt = (JObject)JsonConvert.DeserializeObject(xsxtStr);
                //var xs = xsxt["X-s"].ToString();
                //var xt = xsxt["X-t"].ToString();
            }
        }
        /// <summary>
        /// 加载完页面时，获取cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //TODO:自动获取cookie
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

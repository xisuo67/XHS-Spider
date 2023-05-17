using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
        private static readonly Service.Log.ILogger Logger = LoggerService.Get(typeof(ScriptHost));
        private WebView2 webView;
        private Dictionary<string, string> mCookies = new Dictionary<string, string>();//保存Cookie到字典中
        private string mUrl;
        public static ScriptHost scriptHost = null;
        private ScriptHost(WebView2 webView)
        {
            scriptHost = this;
            this.webView = webView;
            //注册事件侦听，加载页面完成时，获取cookie；
            this.webView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webView_NavigationCompleted);
            //webView初始化完成后注册与JavaScript交互
            this.webView.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.webView_CoreWebView2InitializationCompleted);
        }
        public static ScriptHost GetScriptHost(WebView2 webView)
        {
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
        /// 加载完页面时，获取cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {

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

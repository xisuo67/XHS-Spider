using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XHS
{
    /// <summary>
    /// 网页调用C#方法
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class ScriptHost
    {

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
            this.webView.WebMessageReceived += WebView_WebMessageReceived;
            //webView初始化完成后注册与JavaScript交互
            this.webView.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.webView_CoreWebView2InitializationCompleted);
        }

        private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
        }

        public static ScriptHost GetScriptHost(WebView2 webView)
        {
            if (scriptHost == null) scriptHost = new ScriptHost(webView);
            return scriptHost;
        }
        public void Sign(string xsxt)
        {
            var a= xsxt;
        }
        /// <summary>
        /// 日志记录（JavaScript前端调用
        /// </summary>
        /// <param name="message">JavaScript前端信息</param>
        public void log(string message)
        {
            //Log.i(message);//记录到文本文件中
            //MessageBox.Show(message);
        }
        /// <summary>
        /// 提取cookie中的一个字段；
        /// </summary>
        /// <param name="url">域名</param>
        /// <param name="key">关键字，如：web_session</param>
        /// <param name="t">延时（没用到）</param>
        /// <returns></returns>
        public string getCookieEx(string url, string key, int t)
        {
            getCookie(url);
            if (mCookies.ContainsKey(key))
            {
                string cookies = "";
                foreach (var cookie in mCookies)
                {
                    cookies += cookie.Key + "=" + cookie.Value + ";";
                }
                cookies = key + "=" + mCookies[key];
                //cookies = "web_session=040069b3b3f6625dade26f8d1d364b44f72187";
                return cookies;
            }


            return null;
        }
        /// <summary>
        /// WebView2异步获取cookie
        /// </summary>
        /// <param name="url">与cookie关联的域名</param>
        private async void getCookie(string url)
        {
            //string cookies = "";
            //var cookis = cookies.Split(';');
            //foreach (var item in cookis)
            //{
            //    var cookieItem = item.Trim();
            //    //二次拆分，求name,value
            //    var cookieItemValue = cookieItem.Split('=');
            //    if (cookieItemValue.Count() == 2)
            //    {
            //        CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookie(cookieItemValue[0], cookieItemValue[1], ".xiaohongshu.com", "/");
            //        webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
            //    }
            //}
            List<CoreWebView2Cookie> cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(url);
            mCookies.Clear();
            for (int i = 0; i < cookieList.Count; ++i)
            {
                CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList[i].ToSystemNetCookie());
                mCookies.Add(cookie.Name, cookie.Value);
            }
        }

        public async void UpdateCookie(Dictionary<string, string> dic)
        {
            foreach (var item in dic)
            {
                CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookie(item.Key, item.Value, ".xiaohongshu.com", "/");
                webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
            }
            mCookies.Clear();
            webView.Reload();
            //string url = "https://www.xiaohongshu.com/explore";
            //List<CoreWebView2Cookie> cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(url);
    
            //for (int i = 0; i < cookieList.Count; ++i)
            //{
            //    CoreWebView2Cookie cookie = webView.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList[i].ToSystemNetCookie());
            //    mCookies.Add(cookie.Name, cookie.Value);
            //}
        }
        public string getCookie()
        {
            string cookies = "";
            foreach (var cookie in mCookies)
            {
                cookies += cookie.Key + "=" + cookie.Value + ";";
            }
            return cookies;
        }
        /// <summary>
        /// 记录由JavaScript返回的信息
        /// </summary>
        /// <param name="text"></param>
        public void getJsResult(string text)
        {
            //fMainForm.GetFMainForm().syncContext.Send(fMainForm.GetFMainForm().SetTextSafePost, text);//Post 将信息发送到窗体显示
        }
        //---------------------------------------------------------socket--------------------------------------------------------------------------------
        /// <summary>
        /// 重定向链接，该方法由JavaScript调用；
        /// </summary>
        /// <param name="url"></param>
        /// <param name="domain"></param>
        /// <param name="ua"></param>
        /// <returns></returns>
        public string getRedirectedUrl(string url, string domain, string ua)
        {
            //return NetHelper.getRedirectedUrl(url, domain, ua);
            return "";
        }
        /// <summary>
        /// 提交post请求，该方法由JavaScript调用；
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string getPostResultEx(string url, string args, string headers)
        {
            //log(url);
            //log(args);
            //log(headers);
            //return NetHelper.getPostResult(url, args, headers);
            return "";
        }

        public string getRequestResultEx(string url, string headers)
        {
            //return NetHelper.getHtmlCodeEx(url, headers);
            return "";
        }

        //-----------------------------------------------------------事件---------------------------------------------------------------------------
        /// <summary>
        /// 加载完页面时，获取cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {

            string url = webView.Source.Host.ToString();
            url = "https://" + url;
            getCookie(url);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using XHS.Common.Global;
using System.Windows.Documents;
using Newtonsoft.Json;
using XHS.Common.Helpers;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using Downloader;
using System.Windows;

namespace XHS.Common.Http
{
    public class HttpClientHelper
    {
        private static string _baseUrl = "https://edith.xiaohongshu.com";
        private static HttpClient _client = new HttpClient(new HttpClientHandler
        {
            UseDefaultCredentials = false,
            AllowAutoRedirect = false,
            UseCookies = false,
            Proxy = null,
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.GZip
        });
        /// <summary>
        /// httpPost
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">可选参数</param>
        /// <returns>请求结果</returns>
        public static string DoPost(string url, string postData = "")
        {
            try
            {
                string apiUrl = _baseUrl + url;
                var requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = new Uri(apiUrl);
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(postData, Encoding.UTF8, "application/json");
                var task = Task.Factory.StartNew(() =>
                {
                    return Task.Run(() =>
                    {
                        AddDefaultHeaders(url, postData);
                        var response = _client.SendAsync(requestMessage);
                        return response;
                    }).GetAwaiter().GetResult();
                });
                task.Wait();
                var taskNew = Task.Factory.StartNew(() =>
                {
                    return Task.Run(task.Result.Content.ReadAsStringAsync).GetAwaiter().GetResult();
                });
                taskNew.Wait();
                return taskNew.Result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string DoGet(string url, string postData = "")
        {
            try
            {
                string apiUrl = _baseUrl + url;
                Console.WriteLine("接口调用地址：" + apiUrl + "\n");
                var requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = new Uri(apiUrl);
                requestMessage.Method = HttpMethod.Get;
                var task = Task.Factory.StartNew(() =>
                {
                    return Task.Run(() =>
                    {
                        AddDefaultHeaders(url, postData);
                        var response = _client.SendAsync(requestMessage);
                        return response;
                    }).GetAwaiter().GetResult();
                });
                task.Wait();
                var taskNew = Task.Factory.StartNew(() =>
                {
                    return Task.Run(task.Result.Content.ReadAsStringAsync).GetAwaiter().GetResult();
                });
                taskNew.Wait();
                return taskNew.Result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        private static async void AddDefaultHeaders(string url, string postData = "")
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
            //TODO:cookie通过手动设置
            var cookies = GlobalCaChe.Cookies;
            if (cookies.Count > 0)
            {
                var random = new Random().Next(cookies.Count);
                string cookie = cookies[random]?.Cookie;
                _client.DefaultRequestHeaders.Add("cookie", cookie);
            }
            try
            {
                //TODO:注入js脚本，获取xs、xt;
                var xsxtFilePath = FileHelper.GetAbsolutePath("/Script/XHS-XSXT.js");
                var xsxtCode = File.ReadAllText(xsxtFilePath);
                string jscode = "var url='" + url + "';\r\n" + xsxtCode;
                var xsxtStr = await GlobalCaChe.webView.CoreWebView2.ExecuteScriptAsync(jscode);
                
                if (!string.IsNullOrEmpty(xsxtStr))
                {
                    JObject xsxt = (JObject)JsonConvert.DeserializeObject(xsxtStr);
                    var xs = xsxt["X-s"].ToString();
                    var xt = xsxt["X-t"].ToString();
                    _client.DefaultRequestHeaders.Add("x-s", xs);
                    _client.DefaultRequestHeaders.Add("x-t", xt);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;

namespace XHS.Common.Http
{
    public class HttpClientHelper
    {
        private static string _baseUrl = "https://edith.xiaohongshu.com";
        public static HttpClient _client = new HttpClient(new HttpClientHandler
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
        private static void AddDefaultHeaders(string url, string postData = "")
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
            //TODO:cookie通过手动设置
            _client.DefaultRequestHeaders.Add("cookie", "");
            using (var engine = new V8ScriptEngine())
            {
                engine.DocumentSettings.AccessFlags = Microsoft.ClearScript.DocumentAccessFlags.EnableFileLoading;
                engine.DefaultAccess = Microsoft.ClearScript.ScriptAccess.Full; // 这两行是为了允许加载js文件
                string originScript = System.IO.File.ReadAllText("Script/origin_script.js", System.Text.Encoding.UTF8);
                engine.Execute(originScript);
                ////直接C#函数调用
                var rValue = engine.Script.sign(url, string.IsNullOrEmpty(postData) ? null : postData);
                var dic = rValue as IDictionary<string, object>;
                if (dic!=null)
                {
                    foreach (var item in dic)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                    }
                }
            }
        }
    }
}

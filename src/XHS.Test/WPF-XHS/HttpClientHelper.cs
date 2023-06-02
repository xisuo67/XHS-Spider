using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XHS
{
    public class HttpClientHelper
    {
        public static string cookie = string.Empty;
        public static string xs = string.Empty;
        public static string xt = string.Empty;
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
                        //AddDefaultHeaders(url);
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
            //var cookie = Getcookie().GetAwaiter().GetResult();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
            _client.DefaultRequestHeaders.Add("cookie",cookie);
            _client.DefaultRequestHeaders.Add("x-s", xs);
            _client.DefaultRequestHeaders.Add("x-t", xt);

        }
        /// <summary>
        /// httpPost
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">可选参数</param>
        /// <returns>请求结果</returns>
        public static string DoPostTest(string url, string postData = "")
        {
            //https://cloud.tencent.com/developer/beta/article/1986113
            //https://blog.csdn.net/m0_46639364/article/details/123801059
            try
            {
                string apiUrl = url;
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
    }
}

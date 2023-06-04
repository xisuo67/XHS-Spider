using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_XHS.Model;

namespace WPF_XHS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SynchronizationContext syncContext = null;
        //private static fMainForm mainForm = null;
        private ScriptHost scriptHost = null;
        public MainWindow()
        {
            //Source = "https://www.xiaohongshu.com/explore"
           
            InitializeComponent();
            webView.Source = new Uri("https://www.xiaohongshu.com/explore");
            InitializeAsync();
        }
        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            Config.GetConfig();
         

            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            scriptHost = ScriptHost.GetScriptHost(webView);
        }

        private async void test_click(object sender, RoutedEventArgs e)
        {
            string url = "/api/sns/web/v1/user_posted?num=30&cursor=&user_id=6428e75c0000000012010c39";
            string jscode = "var url='" + url + "';\r\n var t=null;" + Config.jscode;
            HttpClientHelper.cookie = scriptHost.getCookie();
            var xsxt=  await webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxt))
            {
                var sign= JsonConvert.DeserializeObject<XsXt>(xsxt);
                HttpClientHelper.xs = sign.Xs;
                HttpClientHelper.xt = sign.Xt.ToString();
                string result = HttpClientHelper.DoGet(url + "\n");
                MessageBox.Show(result);
            }
        }

        private async void testSearch_click(object sender, RoutedEventArgs e)
        {
            string url = "/api/sns/web/v1/search/notes";
            SearchInputModel model = new SearchInputModel()
            {
                keyword = "美女",
                search_id = AlgorithmHelper.GetSearchId()
            };
            var data= JsonConvert.SerializeObject(model);

            string jscode = "var url='" + url + "';\r\n var jsonStr='" + data+ "';var t = JSON.parse(jsonStr);" + Config.jscode;
            HttpClientHelper.cookie = scriptHost.getCookie();
            var xsxt = await webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxt))
            {
                var sign = JsonConvert.DeserializeObject<XsXt>(xsxt);
                HttpClientHelper.xs = sign.Xs;
                HttpClientHelper.xt = sign.Xt.ToString();
                string result = HttpClientHelper.DoPost(url + "\n",data);
                MessageBox.Show(result);
            }
        }

        private async void qrcode_click(object sender, RoutedEventArgs e)
        {
            string url = "/api/sns/web/v1/login/qrcode/create";
            string jscode = "var url='" + url + "';\r\n var t=null;" + Config.jscode;
            HttpClientHelper.cookie = scriptHost.getCookie();
            var xsxt = await webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxt))
            {
                var sign = JsonConvert.DeserializeObject<XsXt>(xsxt);
                HttpClientHelper.xs = sign.Xs;
                HttpClientHelper.xt = sign.Xt.ToString();
                string result = HttpClientHelper.DoPost(url + "\n");
                var data= JsonConvert.DeserializeObject<XHSBaseApiModel<QrCode>>(result);
                var qrcode = GetLoginQRCode(data.Data.url);
                nameLoginQRCode.Source = qrcode;
                await GetStatus(data.Data);
            }
        }
        //string url= "/api/sns/web/v2/user/me" get方法  
        private async Task<bool> GetStatus(QrCode qrCode)
        {
            string url = $"/api/sns/web/v1/login/qrcode/status?qr_id={qrCode.qr_id}&code={qrCode.code}";
            string jscode = "var url='" + url + "';\r\n var t=null;" + Config.jscode;
            HttpClientHelper.cookie = scriptHost.getCookie();
            var xsxt = await webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxt))
            {
                var sign = JsonConvert.DeserializeObject<XsXt>(xsxt);
                HttpClientHelper.xs = sign.Xs;
                HttpClientHelper.xt = sign.Xt.ToString();
                string result = HttpClientHelper.DoGet(url + "\n");
                var data = JsonConvert.DeserializeObject<XHSBaseApiModel<LoginInfoStatus>>(result);
                //登录成功刷新cookie
                if (data.Data.code_status==2)
                {
                    Dictionary<string,string> dic=new Dictionary<string,string>();
                    dic.TryAdd("web_session",data.Data.login_info.session);
                    scriptHost.UpdateCookie(dic);
                    return true;
                }
                //TODO:判断状态，退出循环，设置cookie，web_session，并查询用户信息
            }
            await Task.Delay(10000);
            GetStatus(qrCode).GetAwaiter();
            return false;
        }
        private async void GetCurrentUser() {
            string url = "/api/sns/web/v2/user/me";
            string jscode = "var url='" + url + "';\r\n var t=null;" + Config.jscode;
            HttpClientHelper.cookie = scriptHost.getCookie();
            var xsxt = await webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxt))
            {
                var sign = JsonConvert.DeserializeObject<XsXt>(xsxt);
                HttpClientHelper.xs = sign.Xs;
                HttpClientHelper.xt = sign.Xt.ToString();
                string result = HttpClientHelper.DoGet(url + "\n");
                MessageBox.Show(result);
            }
        }
        /// <summary>
        /// 根据输入url生成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static BitmapImage GetLoginQRCode(string url)
        {
            // 设置的参数影响app能否成功扫码
            System.Drawing.Bitmap qrCode = QRCode.EncodeQRCode(url, 10, 10, null, 0, 0, false);

            MemoryStream ms = new MemoryStream();
            qrCode.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();
            ms.Close();

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            return image;
        }

        private void currentUser_click(object sender, RoutedEventArgs e)
        {
            GetCurrentUser();
        }
    }
    public class SearchInputModel
    {
        public string keyword { get; set; }
        /// <summary>
        /// 笔记类型 0:全部，1：视频，2：笔记
        /// </summary>
        public int note_type { get; set; } = 0;
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; } = 1;
        /// <summary>
        /// 条数
        /// </summary>
        public int page_size { get; set; } = 20;

        public string search_id { get; set; }
        /// <summary>
        /// 排序字段：默认：general，最新：time_descending，最热：popularity_descending，
        /// </summary>
        public string sort { get; set; } = "general";
    }
}

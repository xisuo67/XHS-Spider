using Hardcodet.Wpf.TaskbarNotification;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Helpers;
using XHS.IService.XHS;
using XHS.Models.XHS.ApiOutputModel.CreateQrCode;
using XHS.Service.Log;
using XHS.Service.XHS;
using XHS.Spider.Helpers;
using XHS.Spider.ViewModels;
using static EdgeSharp.Interop;

namespace XHS.Spider.Views.Windows
{
    /// <summary>
    /// ScanLogin.xaml 的交互逻辑
    /// </summary>
    public partial class ScanLogin : Window, INavigationWindow
    {
        private static readonly Service.Log.ILogger Logger = LoggerService.Get(typeof(ScanLogin));
        #region 属性
        private readonly TaskbarIcon _notifyIcon;
        private readonly IXhsSpiderService _xhsSpiderService;
        private ScriptHost _scriptHost;
        private QrCodeModel _qrcodeInfo;
        #endregion
        public ScanLogin(IXhsSpiderService xhsSpiderService)
        {
            _notifyIcon = new TaskbarIcon();
            _xhsSpiderService = xhsSpiderService;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
            _scriptHost=  ScriptHost.scriptHost;
            InitializeComponent();
        }
        public async Task<QrCodeModel> InitQrCode()
        {
            QrCodeModel qrcodeInfo = null;
            try
            {
                qrcodeInfo = await _xhsSpiderService.CreateQrCode();
                if (qrcodeInfo != null)
                {
                    var qrcode = GetLoginQRCode(qrcodeInfo.Url);
                    if (qrcode != null)
                    {
                        //App.PropertyChangeAsync(new Action(async () =>
                        //{

                        //}));
                        this.nameLoginQRCode.Source = qrcode;
                       
                        //TODO:二维码设置成功后，轮询状态
                        //var isLogin= await GetStatus(qrcodeInfo);
                        // if (isLogin) {
                        //     //登录成功，提示登录成功，并关闭扫码扫码界面；
                        //     _notifyIcon.ShowBalloonTip("扫码登录成功", "提示", BalloonIcon.None);
                        //     this.Close();
                        // }
                    }
                }
                else
                {
                    _notifyIcon.ShowBalloonTip("二维码获取失败", "提示", BalloonIcon.Error);
                }
            }
            catch (Exception ex)
            {
                //Wpf.Ui.Controls.MessageBox.("二维码获取失败");
                Logger.Error("二维码获取失败", ex);
            }
            return qrcodeInfo;
        }
        public async Task<bool> GetStatus(QrCodeModel qrCode)
        {
            var data = _xhsSpiderService.GetStatus(qrCode).Result;
            if (data != null)
            {
                //登录成功刷新cookie
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.TryAdd("web_session", data.LoginInfo.Session);
                if (_scriptHost != null)
                {
                    _scriptHost.UpdateCookie(dic);
                }
                //TODO:判断状态，退出循环，设置cookie，web_session，并查询用户信息
                return true;
            }
            else
            {
                await Task.Delay(1000);
                GetStatus(qrCode).GetAwaiter();
            }
            return false;
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
        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public Frame GetFrame()
        {
            throw new NotImplementedException();
        }

        public INavigation GetNavigation()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow()
        {
            throw new NotImplementedException();
        }

        private async void ScanFrm_Load(object sender, RoutedEventArgs e)
        {
            _qrcodeInfo =await InitQrCode();
        }
    }
}

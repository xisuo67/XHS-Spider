using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using XHS.Common.Events;
using XHS.Common.Events.Model;
using XHS.Common.Global;
using XHS.Common.Helpers;
using XHS.IService.XHS;
using XHS.Models.XHS.ApiOutputModel.CreateQrCode;
using XHS.Service.Log;
using XHS.Spider.Helpers;

namespace XHS.Spider.Views.Windows
{
    /// <summary>
    /// ScanLogin.xaml 的交互逻辑
    /// </summary>
    public partial class ScanLogin : Window
    {
        private static readonly Service.Log.ILogger Logger = LoggerService.Get(typeof(ScanLogin));
        #region 属性
        private readonly TaskbarIcon _notifyIcon;
        private IEventAggregator _aggregator { get; set; }
        private readonly IXhsSpiderService _xhsSpiderService;
        private ScriptHost _scriptHost;
        #endregion
        public ScanLogin(IXhsSpiderService xhsSpiderService, IEventAggregator aggregator)
        {
            _notifyIcon = new TaskbarIcon();
            _aggregator = aggregator;
            _xhsSpiderService = xhsSpiderService;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
            _scriptHost=  ScriptHost.scriptHost;
            InitializeComponent();
            _aggregator.GetEvent<GetQrCodeStatusCallbackEvent>().Subscribe(GetStatus);
            this.Closed += ScanLogin_Closed;
        }
        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ScanLogin_Closed(object? sender, EventArgs e)
        {
            //卸载订阅事件
            _aggregator.GetEvent<GetQrCodeStatusCallbackEvent>().Unsubscribe(GetStatus);
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
                        this.nameLoginQRCode.Source = qrcode;
                    }
                }
                else
                {
                    _notifyIcon.ShowBalloonTip("二维码获取失败", "提示", BalloonIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("二维码获取失败", ex);
            }
            return qrcodeInfo;
        }
        public async void GetStatus(QrCodeModel qrCode)
        {
            await Task.Delay(1000);
            var data = await _xhsSpiderService.GetStatus(qrCode);
            if (data != null)
            {
                //登录成功刷新cookie
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.TryAdd("web_session", data.LoginInfo.Session);
                //TODO:判断状态，退出循环，设置cookie，web_session，并查询用户信息
                if (_scriptHost != null)
                {
                    _scriptHost.UpdateCookie(dic);
                    //TODO:获取当前登录用户
                    int index = 1;
                    GetCurrentUser(index);
                    //登录成功，提示登录成功，并关闭扫码扫码界面；
                    _notifyIcon.ShowBalloonTip("扫码登录成功", "提示", BalloonIcon.None);
                    this.Close();
                    return;
                }
            }
            else
            {
                GetStatus(qrCode);
            }
        }
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <param name="reTryCount">重试次数</param>
        private async void GetCurrentUser(int reTryCount) {

            var currentUser = await _xhsSpiderService.GetCurrentUser();
            //由于更新cookie需要后需要一定时间等待cookie刷新，会出现一定几率获取不到当前登录用户数据，需要增加重试机制
            if (currentUser != null)
            {
                GlobalCaChe.CurrentUser = currentUser;
            }
            else
            {
                //3次以后不管了，不获取当前登录信息了
                if (reTryCount==3)
                {
                    return;
                }
                else
                {
                    reTryCount++;
                    GetCurrentUser(reTryCount);
                }
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

        private async void ScanFrm_Load(object sender, RoutedEventArgs e)
        {
            var qrcodeInfo =await InitQrCode();
            //执行回调事件，监听扫码状态，这里不能直接调用获取状态接口，不然fromLoad事件由于递归原因，永远无法执行完，导致二维码没法显示
            _aggregator.GetEvent<GetQrCodeStatusCallbackEvent>().Publish(qrcodeInfo);
        }
    }
}

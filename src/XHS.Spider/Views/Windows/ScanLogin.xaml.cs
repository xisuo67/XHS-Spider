using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using XHS.Service.Log;
using XHS.Service.XHS;
using XHS.Spider.Helpers;
using XHS.Spider.ViewModels;

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
        #endregion
        public ScanLogin(IXhsSpiderService xhsSpiderService)
        {
            _notifyIcon = new TaskbarIcon();
            _xhsSpiderService = xhsSpiderService;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
            InitializeComponent();
            InitQrCode();
        }
        public async void InitQrCode()
        {
            try
            {
                var qrcodeInfo = await _xhsSpiderService.CreateQrCode();
                if (qrcodeInfo != null)
                {
                    var qrcode = GetLoginQRCode(qrcodeInfo.Url);
                    if (qrcode != null)
                    {
                        App.PropertyChangeAsync(new Action(async () =>
                        {
                            this.nameLoginQRCode.Source = qrcode;
                        }));
                        //TODO:二维码设置成功后，轮询状态
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
    }
}

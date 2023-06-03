using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using XHS.Common.Helpers;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// 扫码登录
    /// </summary>
    public partial class ScanLoginViewModel : ObservableObject, INavigationAware
    {
        #region 属性
        public BitmapImage QrCodeImage { get; set; }
        #endregion

        public ScanLoginViewModel(string url) {
            QrCodeImage = GetLoginQRCode(url);
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
        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
        }
    }
}

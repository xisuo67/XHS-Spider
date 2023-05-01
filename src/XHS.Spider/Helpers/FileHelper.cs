using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XHS.Spider.Helpers
{
    public class FileHelper
    {
        public static BitmapImage UrlToBitmapImage(string url)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetStreamAsync(url).GetAwaiter().GetResult();
            Image image = Image.FromStream(result);
            Bitmap bitmap = new Bitmap(image);
            return BitmapToBitmapImage(bitmap);
        }
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}

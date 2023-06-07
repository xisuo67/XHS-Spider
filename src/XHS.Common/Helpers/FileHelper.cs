using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XHS.Common.Helpers
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
        /// <summary>
        /// 获取文件的绝对路径,针对window程序和web程序都可使用
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>绝对路径地址</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>bool</returns>
        public static bool Exists(string relativePath)
        {
            return File.Exists(GetAbsolutePath(relativePath));
        }
        public static BitmapImage PathToBitmapImage(string filePath)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + filePath;
                Image image = Image.FromFile(path);
                Bitmap bitmap = new Bitmap(image);
                return BitmapToBitmapImage(bitmap);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string TransTime(long str)
        {
            DateTime nowTime;
            if (str.ToString().Length == 13)
            {
                nowTime = new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(str);
            }
            else
            {
                nowTime = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(str);
            }
            return nowTime.ToString("yyyy-MM-dd HH-mm-ss");
        }
        public static void CreatTxtFile(string filePath, string str)
        {
            /* 生成文件名 */
            string path = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(filePath, str);
        }

    }
}

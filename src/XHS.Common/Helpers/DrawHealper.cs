using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Helpers
{
    public class DrawHealper
    {
        #region 绘制头像框
        /// <summary>
        /// 生成圆形头像
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static Bitmap CreateHead(string text)
        {
            Bitmap bitmap = new Bitmap(50, 50);
            var font = new Font("文泉驿正黑", 8, FontStyle.Bold);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            //画圆
            Brush bush = new SolidBrush(ColorTranslator.FromHtml("#C0C0C0"));//填充的颜色
            g.FillEllipse(bush, rect);
            //文字居中
            SizeF size = g.MeasureString(text, font);
            int nLeft = Convert.ToInt32((bitmap.Width / 2) - (size.Width / 2));
            int nTop = Convert.ToInt32((bitmap.Height / 2) - (size.Height / 2));
            g.DrawString(text, font, new SolidBrush(ColorTranslator.FromHtml("#50A0FF")), nLeft, nTop);

            //MemoryStream ms = new MemoryStream();
            var bmp = CutEllipse(bitmap, rect, bitmap.Size);
            return bmp;
            //return ms;
        }

        /// <summary>
        /// 剪裁圆形
        /// </summary>
        /// <param name="img"></param>
        /// <param name="rec"></param>
        /// <param name="size"></param>
        /// <param name="imgSavePath"></param>
        /// <returns></returns>
        private static Bitmap CutEllipse(Image img, Rectangle rec, Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (TextureBrush br = new TextureBrush(img, WrapMode.Clamp, rec))
                {
                    br.ScaleTransform(bitmap.Width / (float)rec.Width, bitmap.Height / (float)rec.Height);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillEllipse(br, new Rectangle(Point.Empty, size));
                }
            }
            return bitmap;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Utils
{
    public class Utils
    {
        /// <summary>
        /// 打开地址
        /// </summary>
        /// <param name="path"></param>
        public static void OpenURL(string path)
        {
            new Process
            {
                StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
        /// <summary>
        /// 计算文件夹下文件数量
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        public static int GetFilesCount(DirectoryInfo dirInfo)
        {

            int totalFile = 0;
            totalFile += dirInfo.GetFiles().Length;//获取全部文件
            foreach (System.IO.DirectoryInfo subdir in dirInfo.GetDirectories())
            {
                totalFile += GetFilesCount(subdir);
            }
            return totalFile;
        }
    }
}

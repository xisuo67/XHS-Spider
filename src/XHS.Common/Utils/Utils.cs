using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Utils
{
    public class Utils
    {
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
    }
}

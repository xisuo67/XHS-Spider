using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XHS
{
    internal class Config
    {
        public const string app_name = "xhsreport";
        public const string app_title = "小红书指标接口解析";
        public const string app_version = "1.0";
        public const string app_id = "cf";
        public const string app_author = "微信;metabycf";
        public const bool DEBUG = true;

        private const string WORK_DIR = "xhsreport";
        private const string LOG_FILE = "xhsreportlog.txt";
        private const string SAVE_DIR = "xhsreport";
        private const string JS_TEST = "script/xhsign_new.js";//xhs_report.js
        private const string CONFIG_FILE = "xhsreport.config";

        public const string JS_ADDR = "http://helpnow.top/search.js";

        public static string logfile = "";
        public static string workdir = "";
        public static string savedir = "";
        public static string jscode = "";
        public static string jstest = "";
        public static string configfile = "";

        private static Config current;
        private Config()
        {
            workdir = Environment.CurrentDirectory; //+ "\\" + WORK_DIR;
            if (!Directory.Exists(workdir))
                Directory.CreateDirectory(workdir);
            savedir = workdir + "\\" + SAVE_DIR;
            if (!Directory.Exists(savedir))
                Directory.CreateDirectory(savedir);

            logfile = workdir + "\\" + LOG_FILE;

            //Log.GetLog(logfile);

            jstest = workdir + "\\" + JS_TEST;
            getJsCode();

            configfile = workdir + "\\" + CONFIG_FILE;
            readConfig();


        }
        public static Config GetConfig()
        {
            if (current == null) current = new Config();
            return current;
        }

        public void getJsCode()
        {
            if (DEBUG)
            {
                jscode = System.IO.File.ReadAllText(jstest);
            }
            else
            {
                //jscode = NetHelper.getHtmlCode(JS_ADDR);
            }
        }

        private void readConfig()
        {
            if (File.Exists(configfile))
            {
                savedir = System.IO.File.ReadAllText(configfile);
            }
            else
            {
                File.WriteAllText(configfile, savedir);
            }
        }
    }
}

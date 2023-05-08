using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XHS.Service.Log
{
    public class LoggerService : ILogger
    {
        private static readonly IDictionary<Type, ILogger> Loggers = new ConcurrentDictionary<Type, ILogger>();

        public static Func<Type, ILogger> LoggerFactory { get; set; }

        public static ILogger Get(Type type)
        {
            if (!Loggers.ContainsKey(type))
            {
                Loggers[type] = LoggerFactory?.Invoke(type) ?? new LoggerService(type);
            }

            return Loggers[type];
        }

        private readonly Type _type;

        protected LoggerService(Type type) => _type = type;

        public void Fatal(string message, Exception e = null)
        {
            Write(nameof(Fatal), message, e);
        }

        public void Error(string message, Exception e = null)
        {
            Write(nameof(Error), message, e);
        }

        public void Warning(string message, Exception e = null)
        {
            Write(nameof(Warning), message, e);
        }

        public void Info(string message, Exception e = null)
        {
            Write(nameof(Info), message, e);
        }

        public void Debug(string message, Exception e = null)
        {
            Write(nameof(Debug), message, e);
        }

        private void Write(string category, string message, Exception e)
        {
            message = $"{message}{Environment.NewLine}";

            if (e != null)
            {
                message = $"{message}{e.Message}{Environment.NewLine}" +
                          $"{e.StackTrace}{Environment.NewLine}";
            }

            message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} " +
                      $"[{Thread.CurrentThread.ManagedThreadId}] " +
                      $"{category.ToUpper()} " +
                      $"{_type.FullName}{Environment.NewLine}" +
                      $"{message}{Environment.NewLine}";

            System.Diagnostics.Debug.Write(message);
            var path = GetFileFullPath($"log/{DateTime.Now:yyyy-MM-dd}", "run.log");
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(message);
                //sw.Write(message);
            }
        }

        /// <summary>
        /// 获取完整文件路径（如果文件夹不存在，创建文件夹）
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileFullPath(string directoryName, string fileName)
        {
            string runPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = runPath + "/" + directoryName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fullPath = Path.Combine(path, fileName);
            return fullPath;
        }
    }
}

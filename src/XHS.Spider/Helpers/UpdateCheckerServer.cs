using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateChecker;
using UpdateChecker.VersionComparers;
using XHS.Service.Log;

namespace XHS.Spider.Helpers
{
    public class UpdateCheckerServer
    {
        private static readonly ILogger Logger = LoggerService.Get(typeof(UpdateCheckerServer));
        private const string Owner = @"xisuo67";
        private const string Repo = @"XHS-Spider";

        public string LatestVersionNumber;
        public string LatestVersionUrl;

        public bool Found;

        public event EventHandler NewVersionFound;
        public event EventHandler NewVersionFoundFailed;
        public event EventHandler NewVersionNotFound;
        public const string Name = @"XHS-Spider";
        public const string Copyright = @"Copyright © 2023-present xisuo67";

        private static string version = string.Empty;
        public static string Version
        {
            get => version;
            set => version = value;
        }

        public static string FullVersion = Version +
#if SelfContained
#if Is64Bit
            @" x64" +
#else
            @" x86" +
#endif
#endif
#if DEBUG
        @" Debug";
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateCheckerServer()
        {
            version = GetAssemblyVersion();
        }
#else
        @"";
#endif

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? @"1.0.0.0";
        }
        public async void Check(bool notifyNoFound)
        {
            try
            {
                var updater = new GitHubReleasesUpdateChecker(
                    Owner,
                    Repo,
                    false,
                    Version,
                    tag => tag.Replace(@"v", string.Empty),
                    new DefaultVersionComparer());
                var res = await updater.CheckAsync(default);
                LatestVersionNumber = updater.LatestVersion;
                Found = res;
                if (Found)
                {
                    LatestVersionUrl = updater.LatestVersionUrl;
                    NewVersionFound?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    if (notifyNoFound)
                    {
                        NewVersionNotFound?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                if (notifyNoFound)
                {
                    NewVersionFoundFailed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}

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
    public class UpdateChecker
    {
        private static readonly ILogger Logger = LoggerService.Get(typeof(UpdateChecker));
        private const string Owner = @"xisuo67";
        private const string Repo = @"Daily-Newspaper-Tools";

        public string LatestVersionNumber;
        public string LatestVersionUrl;

        public bool Found;

        public event EventHandler NewVersionFound;
        public event EventHandler NewVersionFoundFailed;

        public const string Name = @"Daily-Newspaper";
        public const string Copyright = @"Copyright © 2022-present xisuo67";
        public const string Version = @"1.0.1";

        public const string FullVersion = Version +
#if SelfContained
#if Is64Bit
            @" x64" +
#else
            @" x86" +
#endif
#endif
#if DEBUG
        @" Debug";

        public UpdateChecker()
        {
        }
#else
        @"";
#endif

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

using System;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Mono.EnvironmentInfo
{
    public class MonoRuntimeProvider : RuntimeInfoBase
    {
        private static readonly Regex VersionRegex = new Regex(@"(?<=\W|^)(?<version>\d+\.\d+(\.\d+)?(\.\d+)?)(?=\W)", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public MonoRuntimeProvider(Common.IServiceProvider serviceProvider, Logger logger)
            :base(serviceProvider, logger)
        {
            var runTimeVersion = new Version();

            try
            {
                var type = Type.GetType("Mono.Runtime");

                if (type != null)
                {
                    var displayNameMethod = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                    if (displayNameMethod != null)
                    {
                        var displayName = displayNameMethod.Invoke(null, null).ToString();
                        var versionMatch = VersionRegex.Match(displayName);

                        if (versionMatch.Success)
                        {
                            runTimeVersion = new Version(versionMatch.Groups["version"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to get mono version: " + ex.Message);
            }


            RuntimeVersion = runTimeVersion;
        }


        public override Version RuntimeVersion { get; }
    }
}

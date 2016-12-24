using System;
using Microsoft.Win32;
using NLog;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Windows.EnvironmentInfo
{
    public class DotNetRuntimeProvider : RuntimeInfoBase
    {
        public DotNetRuntimeProvider(Common.IServiceProvider serviceProvider, Logger logger)
            : base(serviceProvider, logger)
        {
            RuntimeVersion = GetFrameworkVersion();
        }

        public override string RuntimeVersion { get; }

        private static string GetFrameworkVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey == null)
                {
                    return "4.0";
                }

                var releaseKey = (int)ndpKey.GetValue("Release");


                if (releaseKey >= 394802)
                {
                    return "4.6.2";
                }
                else if (releaseKey >= 394254)
                {
                    return "4.6.1";
                }
                else if (releaseKey >= 393295)
                {
                    return "4.6";
                }
                else if (releaseKey >= 379893)
                {
                    return "4.5.2";
                }
                else if (releaseKey >= 378675)
                {
                    return "4.5.1";
                }
                else if (releaseKey >= 378389)
                {
                    return "4.5";
                }

                return "4.0";
            }
        }
    }
}

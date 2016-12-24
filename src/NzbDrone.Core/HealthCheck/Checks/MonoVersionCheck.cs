using System;
using System.Linq;
using System.Reflection;
using NLog;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Core.HealthCheck.Checks
{
    public class MonoVersionCheck : HealthCheckBase
    {
        private readonly IRuntimeInfo _runtimeInfo;
        private readonly Logger _logger;

        public MonoVersionCheck(IRuntimeInfo runtimeInfo, Logger logger)
        {
            _runtimeInfo = runtimeInfo;
            _logger = logger;
        }

        public override HealthCheck Check()
        {
            if (OsInfo.IsWindows)
            {
                return new HealthCheck(GetType());
            }

            var monoVersion = _runtimeInfo.RuntimeVersion;

            if (monoVersion == new Version(3, 4, 0) && HasMonoBug18599())
            {
                _logger.Debug("mono version 3.4.0, checking for mono bug #18599 returned positive.");
                return new HealthCheck(GetType(), HealthCheckResult.Error, "your mono version 3.4.0 has a critical bug, you should upgrade to a higher version");
            }

            if (monoVersion == new Version(4, 4, 0) || monoVersion == new Version(4, 4, 1))
            {
                _logger.Debug("mono version {0}", monoVersion);
                return new HealthCheck(GetType(), HealthCheckResult.Error, $"your mono version {monoVersion} has a bug that causes issues connecting to indexers/download clients");
            }

            if (monoVersion >= new Version(3, 10))
            {
                _logger.Debug("mono version is 3.10 or better: {0}", monoVersion.ToString());
                return new HealthCheck(GetType());
            }

            return new HealthCheck(GetType(), HealthCheckResult.Warning, "mono version is less than 3.10, upgrade for improved stability");
        }

        public override bool CheckOnConfigChange => false;

        public override bool CheckOnSchedule => false;

        private bool HasMonoBug18599()
        {
            _logger.Debug("mono version 3.4.0, checking for mono bug #18599.");
            var numberFormatterType = Type.GetType("System.NumberFormatter");

            if (numberFormatterType == null)
            {
                _logger.Debug("Couldn't find System.NumberFormatter. Aborting test.");
                return false;
            }

            var fieldInfo = numberFormatterType.GetField("userFormatProvider",
                BindingFlags.Static | BindingFlags.NonPublic);

            if (fieldInfo == null)
            {
                _logger.Debug("userFormatProvider field not found, version likely preceeds the official v3.4.0.");
                return false;
            }

            if (fieldInfo.GetCustomAttributes(false).Any(v => v is ThreadStaticAttribute))
            {
                _logger.Debug("userFormatProvider field doesn't contain the ThreadStatic Attribute, version is affected by the critical bug #18599.");
                return true;
            }

            return false;
        }
    }
}
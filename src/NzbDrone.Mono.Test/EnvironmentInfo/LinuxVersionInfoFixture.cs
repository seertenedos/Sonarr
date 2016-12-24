using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Mono.EnvironmentInfo;
using NzbDrone.Test.Common;

namespace NzbDrone.Mono.Test.EnvironmentInfo
{
    public class LinuxVersionInfoFixture : TestBase<LinuxVersionInfo>
    {
        [Test]
        public void should_get_version_info()
        {
            Subject.FullName.Should().NotBeNullOrWhiteSpace();
            Subject.Name.Should().NotBeNullOrWhiteSpace();
            Subject.Version.Should().NotBeNullOrWhiteSpace();
        }
    }
}
using System;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Test.Common;

namespace NzbDrone.Windows.EnvironmentInfo
{
    public class DotNetRuntimeProviderFixture : TestBase<DotNetRuntimeProvider>
    {
        [Test]
        public void should_get_framework_version()
        {
            var version = new Version(Subject.RuntimeVersion);
            version.Major.Should().Be(4);
            version.Minor.Should().BeOneOf(0, 5, 6);
        }
    }
}

using System;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Mono.EnvironmentInfo;
using NzbDrone.Test.Common;

namespace NzbDrone.Mono.Test.EnvironmentInfo
{
    public class DotNetRuntimeProviderFixture : TestBase<MonoRuntimeProvider>
    {
        [Test]
        public void should_get_framework_version()
        {
            Subject.RuntimeVersion.Major.Should().Be(4);
            Subject.RuntimeVersion.Minor.Should().BeOneOf(0, 5, 6);
        }
    }
}

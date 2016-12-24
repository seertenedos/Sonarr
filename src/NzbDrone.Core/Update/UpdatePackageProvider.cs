using System;
using System.Collections.Generic;
using NzbDrone.Common.Cloud;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Common.Http;

namespace NzbDrone.Core.Update
{
    public interface IUpdatePackageProvider
    {
        UpdatePackage GetLatestUpdate(string branch, Version currentVersion);
        List<UpdatePackage> GetRecentUpdates(string branch, Version currentVersion);
    }

    public class UpdatePackageProvider : IUpdatePackageProvider
    {
        private readonly IHttpClient _httpClient;
        private readonly IRuntimeInfo _runtimeInfo;
        private readonly IHttpRequestBuilderFactory _requestBuilder;

        public UpdatePackageProvider(IHttpClient httpClient, ISonarrCloudRequestBuilder requestBuilder, IRuntimeInfo runtimeInfo)
        {
            _httpClient = httpClient;
            _runtimeInfo = runtimeInfo;
            _requestBuilder = requestBuilder.Services;
        }

        public UpdatePackage GetLatestUpdate(string branch, Version currentVersion)
        {
            var request = _requestBuilder.Create()
                                         .Resource("/update/{branch}")
                                         .AddQueryParam("version", currentVersion)
                                         .AddQueryParam("os", OsInfo.Os.ToString().ToLowerInvariant())
                                         .AddQueryParam("osver", OsInfo.Version)
                                         .AddQueryParam("runtime", _runtimeInfo.RuntimeVersion)
                                         .SetSegment("branch", branch)
                                         .Build();

            var update = _httpClient.Get<UpdatePackageAvailable>(request).Resource;

            if (!update.Available) return null;

            return update.UpdatePackage;
        }

        public List<UpdatePackage> GetRecentUpdates(string branch, Version currentVersion)
        {
            var request = _requestBuilder.Create()
                                         .Resource("/update/{branch}/changes")
                                         .AddQueryParam("version", currentVersion)
                                         .AddQueryParam("os", OsInfo.Os.ToString().ToLowerInvariant())
                                         .SetSegment("branch", branch)
                                         .Build();

            var updates = _httpClient.Get<List<UpdatePackage>>(request);

            return updates.Resource;
        }
    }
}
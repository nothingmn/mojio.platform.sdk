﻿using Mojio.Platform.SDK.Contracts;
using Mojio.Platform.SDK.Contracts.Client;
using Mojio.Platform.SDK.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mojio.Platform.SDK
{
    public partial class Client
    {
        public async Task<IPlatformResponse<IAppResponse>> Apps(int skip = 0, int top = 10, string filter = null, string select = null, string orderby = null, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                string path = $"v2/apps?{RandomQueryString()}";
                if (skip > 0) path = path + $"&%24skip={skip}";
                if (top > 0) path = path + $"&%24top={top}";

                if (!string.IsNullOrEmpty(filter)) path = path + $"&%24filter={WebUtility.UrlEncode(filter)}";
                if (!string.IsNullOrEmpty(select)) path = path + $"&%24select={WebUtility.UrlEncode(select)}";
                if (!string.IsNullOrEmpty(orderby)) path = path + $"&%24orderBy={WebUtility.UrlEncode(orderby)}";

                return await CacheHitOrMiss($"Apps.{Authorization.UserName}", () => _clientBuilder.Request<IAppResponse>(ApiEndpoint.Api, path, tokenP.CancellationToken, tokenP.Progress), TimeSpan.FromMinutes(10));
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IAppResponse>>(null);
        }

        public async Task<IPlatformResponse<IApp>> CreateApp(IApp application, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                return await _clientBuilder.Request<IApp>(ApiEndpoint.Api, "v2/apps", tokenP.CancellationToken, tokenP.Progress, HttpMethod.Post, _serializer.SerializeToString(application));
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IApp>>(null);
        }

        public async Task<IPlatformResponse<IApp>> UpdateApp(IApp app, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                return await _clientBuilder.Request<IApp>(ApiEndpoint.Api, string.Format("v2/apps/{0}", app.Id), tokenP.CancellationToken, tokenP.Progress, HttpMethod.Put, _serializer.SerializeToString(app));
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IApp>>(null);
        }

        public async Task<IPlatformResponse<IMessageResponse>> DeleteApp(Guid id, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                return await _clientBuilder.Request<IMessageResponse>(ApiEndpoint.Api, string.Format("v2/apps/{0}", id), tokenP.CancellationToken, tokenP.Progress, HttpMethod.Delete);
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IMessageResponse>>(null);
        }

        public async Task<IPlatformResponse<IMessageResponse>> GetAppSecret(Guid id, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                return await CacheHitOrMiss($"GetAppSecret.{Authorization.UserName}{id}", () => _clientBuilder.Request<IMessageResponse>(ApiEndpoint.Api, string.Format("v2/apps/{0}/secret", id), tokenP.CancellationToken, tokenP.Progress, HttpMethod.Get), TimeSpan.FromMinutes(1));
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IMessageResponse>>(null);
        }

        public async Task<IPlatformResponse<IMessageResponse>> DeleteAppSecret(Guid id, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                var result = await _clientBuilder.Request<IMessageResponse>(ApiEndpoint.Api, string.Format("v2/apps/{0}/secret", id), tokenP.CancellationToken, tokenP.Progress, HttpMethod.Delete);
                await _cache.Delete($"GetAppSecret.{Authorization.UserName}" + id);

                return result;
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IMessageResponse>>(null);
        }
    }
}
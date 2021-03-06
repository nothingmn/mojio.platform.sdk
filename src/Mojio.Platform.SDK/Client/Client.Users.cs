﻿#region copyright
/******************************************************************************
* Moj.io Inc. CONFIDENTIAL
* 2017 Copyright Moj.io Inc.
* All Rights Reserved.
* 
* NOTICE:  All information contained herein is, and remains, the property of 
* Moj.io Inc. and its suppliers, if any.  The intellectual and technical 
* concepts contained herein are proprietary to Moj.io Inc. and its suppliers
* and may be covered by Patents, pending patents, and are protected by trade
* secret or copyright law.
*
* Dissemination of this information or reproduction of this material is strictly
* forbidden unless prior written permission is obtained from Moj.io Inc.
*******************************************************************************/
#endregion

using Mojio.Platform.SDK.Contracts;
using Mojio.Platform.SDK.Contracts.Client;
using Mojio.Platform.SDK.Contracts.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mojio.Platform.SDK
{
    public partial class Client
    {
        public async Task<IPlatformResponse<IUser>> GetUser(Guid userId, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                // TODO: integrate caching

                var fragment = $"v2/users/{userId}";
                return await _clientBuilder.Request<IUser>(ApiEndpoint.Api, fragment, tokenP.CancellationToken, tokenP.Progress);
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IUser>>(null);
        }

        public async Task<IPlatformResponse<IUsersResponse>> Users(int skip = 0, int top = 10, string filter = null, string select = null, string orderby = null, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                string path = $"v2/users?{RandomQueryString()}";
                if (skip > 0) path = path + $"&skip={skip}";
                if (top > 0) path = path + $"&top={top}";

                if (!string.IsNullOrEmpty(filter)) path = path + $"&filter={WebUtility.UrlEncode(filter)}";
                if (!string.IsNullOrEmpty(select)) path = path + $"&select={WebUtility.UrlEncode(select)}";
                if (!string.IsNullOrEmpty(orderby)) path = path + $"&orderby={WebUtility.UrlEncode(orderby)}";

                return await _clientBuilder.Request<IUsersResponse>(ApiEndpoint.Api, path, tokenP.CancellationToken,
                    tokenP.Progress);
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<IUsersResponse>>(null);
        }
    }
}
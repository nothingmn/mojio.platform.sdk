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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mojio.Platform.SDK.Contracts.Machine;

namespace Mojio.Platform.SDK
{
    public partial class Client
    {
        public async Task<IPlatformResponse<string>> Simulate(IMachineRequest request, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                var json = _serializer.SerializeToString(request);
                return await _clientBuilder.Request<string>(ApiEndpoint.Api, "v2/simulator", tokenP.CancellationToken, tokenP.Progress, HttpMethod.Post, json);
            }
            _log.Fatal(new Exception("Authorization Failed"));
            return await Task.FromResult<IPlatformResponse<string>>(null);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mojio.Platform.SDK.Contracts;
using Mojio.Platform.SDK.Contracts.Automation;
using Mojio.Platform.SDK.Contracts.Client;
using Mojio.Platform.SDK.Contracts.Entities;

namespace Mojio.Platform.SDK.Automation.StandardTasks
{
    public abstract class BaseAutomationTask : IAutomationTask
    {
        protected static Random Rnd = new Random();

        public string Key { get; set; }
        public abstract Task Execute(IClient client, IDictionary<string, string> properties);

    }
}
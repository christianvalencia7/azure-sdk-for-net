﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.Dashboard.Tests.Helpers;
using NUnit.Framework;

namespace Azure.ResourceManager.Dashboard.Tests.TestsCase
{
    public class GrafanaResourceOperationsTests : DashboardTestBase
    {
        public GrafanaResourceOperationsTests(bool isAsync)
           : base(isAsync)
        {
        }

        private async Task<GrafanaResource> CreateGrafanaResourceAsync(string name)
        {
            var container = (await CreateResourceGroupAsync()).GetGrafanaResources();
            var input = ResourceDataHelper.GetGrafanaResourceData(DefaultLocation);
            var lro = await container.CreateOrUpdateAsync(WaitUntil.Completed, name, input);
            return lro.Value;
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            var name = Recording.GenerateAssetName("sdkTestGrafana");
            var resource = await CreateGrafanaResourceAsync(name);
            await resource.DeleteAsync(WaitUntil.Completed);
        }
    }
}

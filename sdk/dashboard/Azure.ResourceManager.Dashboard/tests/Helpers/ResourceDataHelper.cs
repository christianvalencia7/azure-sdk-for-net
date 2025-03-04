﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;
using Azure.ResourceManager.Dashboard.Models;
using Azure.ResourceManager.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.Dashboard.Tests.Helpers
{
    public class ResourceDataHelper
    {
        public static GrafanaResourceData GetGrafanaResourceData(AzureLocation location)
        {
            return new GrafanaResourceData(location)
            {
                Sku = new ResourceSku("Standard")
            };
        }

        public static void AssertGrafana(GrafanaResourceData g1, GrafanaResourceData g2)
        {
            AssertTrackedResource(g1, g2);
        }

        public static void AssertTrackedResource(TrackedResourceData r1, TrackedResourceData r2)
        {
            Assert.AreEqual(r1.Name, r2.Name);
            Assert.AreEqual(r1.Id, r2.Id);
            Assert.AreEqual(r1.ResourceType, r2.ResourceType);
            Assert.AreEqual(r1.Location, r2.Location);
            Assert.AreEqual(r1.Tags, r2.Tags);
        }
    }
}

﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.TestFramework;
using Azure.ResourceManager.CosmosDB.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.CosmosDB.Tests
{
    public class CassandraKeyspaceTests : CosmosDBManagementClientBase
    {
        private ResourceIdentifier _keyspaceAccountIdentifier;
        private DatabaseAccountResource _keyspaceAccount;

        private string _keyspaceName;

        public CassandraKeyspaceTests(bool isAsync) : base(isAsync)
        {
        }

        protected CassandraKeyspaceCollection CassandraKeyspaceCollection => _keyspaceAccount.GetCassandraKeyspaces();

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            _resourceGroup = await GlobalClient.GetResourceGroupResource(_resourceGroupIdentifier).GetAsync();

            _keyspaceAccountIdentifier = (await CreateDatabaseAccount(SessionRecording.GenerateAssetName("dbaccount-"), DatabaseAccountKind.GlobalDocumentDB, new DatabaseAccountCapability("EnableCassandra"))).Id;
            await StopSessionRecordingAsync();
        }

        [OneTimeTearDown]
        public virtual void GlobalTeardown()
        {
            if (_keyspaceAccountIdentifier != null)
            {
                ArmClient.GetDatabaseAccountResource(_keyspaceAccountIdentifier).Delete(WaitUntil.Completed);
            }
        }

        [SetUp]
        public async Task SetUp()
        {
            _keyspaceAccount = await ArmClient.GetDatabaseAccountResource(_keyspaceAccountIdentifier).GetAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (await CassandraKeyspaceCollection.ExistsAsync(_keyspaceName))
            {
                var id = CassandraKeyspaceCollection.Id;
                id = CassandraKeyspaceResource.CreateResourceIdentifier(id.SubscriptionId, id.ResourceGroupName, id.Name, _keyspaceName);
                CassandraKeyspaceResource keyspace = this.ArmClient.GetCassandraKeyspaceResource(id);
                await keyspace.DeleteAsync(WaitUntil.Completed);
            }
        }

        [Test]
        [RecordedTest]
        public async Task CassandraKeyspaceCreateAndUpdate()
        {
            var keyspace = await CreateCassandraKeyspace(null);
            Assert.AreEqual(_keyspaceName, keyspace.Data.Resource.Id);
            // Seems bug in swagger definition
            //Assert.AreEqual(TestThroughput1, keyspace.Data.Options.Throughput);

            bool ifExists = await CassandraKeyspaceCollection.ExistsAsync(_keyspaceName);
            Assert.True(ifExists);

            // NOT WORKING API
            //ThroughputSettingsData throughtput = await keyspace.GetMongoDBCollectionThroughputAsync();
            CassandraKeyspaceResource keyspace2 = await CassandraKeyspaceCollection.GetAsync(_keyspaceName);
            Assert.AreEqual(_keyspaceName, keyspace2.Data.Resource.Id);
            //Assert.AreEqual(TestThroughput1, keyspace2.Data.Options.Throughput);

            VerifyCassandraKeyspaces(keyspace, keyspace2);

            // TODO: use original tags see defect: https://github.com/Azure/autorest.csharp/issues/1590
            CassandraKeyspaceCreateUpdateData updateOptions = new CassandraKeyspaceCreateUpdateData(AzureLocation.WestUS, keyspace.Data.Resource)
            {
                Options = new CreateUpdateOptions { Throughput = TestThroughput2 }
            };

            keyspace = (await CassandraKeyspaceCollection.CreateOrUpdateAsync(WaitUntil.Completed, _keyspaceName, updateOptions)).Value;
            Assert.AreEqual(_keyspaceName, keyspace.Data.Resource.Id);
            keyspace2 = await CassandraKeyspaceCollection.GetAsync(_keyspaceName);
            VerifyCassandraKeyspaces(keyspace, keyspace2);
        }

        [Test]
        [RecordedTest]
        public async Task CassandraKeyspaceList()
        {
            var keyspace = await CreateCassandraKeyspace(null);

            var keyspaces = await CassandraKeyspaceCollection.GetAllAsync().ToEnumerableAsync();
            Assert.That(keyspaces, Has.Count.EqualTo(1));
            Assert.AreEqual(keyspace.Data.Name, keyspaces[0].Data.Name);

            VerifyCassandraKeyspaces(keyspaces[0], keyspace);
        }

        [Test]
        [RecordedTest]
        public async Task CassandraKeyspaceThroughput()
        {
            var keyspace = await CreateCassandraKeyspace(null);
            DatabaseAccountCassandraKeyspaceThroughputSettingResource throughput = await keyspace.GetDatabaseAccountCassandraKeyspaceThroughputSetting().GetAsync();

            Assert.AreEqual(TestThroughput1, throughput.Data.Resource.Throughput);

            DatabaseAccountCassandraKeyspaceThroughputSettingResource throughput2 = (await throughput.CreateOrUpdateAsync(WaitUntil.Completed, new ThroughputSettingsUpdateData(AzureLocation.WestUS,
                new ThroughputSettingsResource(TestThroughput2, null, null, null)))).Value;

            Assert.AreEqual(TestThroughput2, throughput2.Data.Resource.Throughput);
        }

        [Test]
        [RecordedTest]
        [Ignore("Need to diagnose The operation has not completed yet.")]
        public async Task CassandraKeyspaceMigrateToAutoscale()
        {
            var keyspace = await CreateCassandraKeyspace(null);

            DatabaseAccountCassandraKeyspaceThroughputSettingResource throughput = await keyspace.GetDatabaseAccountCassandraKeyspaceThroughputSetting().GetAsync();
            AssertManualThroughput(throughput.Data);

            ThroughputSettingsData throughputData = (await throughput.MigrateCassandraKeyspaceToAutoscaleAsync(WaitUntil.Completed)).Value.Data;
            AssertAutoscale(throughputData);
        }

        [Test]
        [RecordedTest]
        [Ignore("Need to diagnose The operation has not completed yet.")]
        public async Task CassandraKeyspaceMigrateToManual()
        {
            var keyspace = await CreateCassandraKeyspace(new AutoscaleSettings()
            {
                MaxThroughput = DefaultMaxThroughput,
            });

            DatabaseAccountCassandraKeyspaceThroughputSettingResource throughput = await keyspace.GetDatabaseAccountCassandraKeyspaceThroughputSetting().GetAsync();
            AssertAutoscale(throughput.Data);

            ThroughputSettingsData throughputData = (await throughput.MigrateCassandraKeyspaceToManualThroughputAsync(WaitUntil.Completed)).Value.Data;
            AssertManualThroughput(throughputData);
        }

        [Test]
        [RecordedTest]
        public async Task CassandraKeyspaceDelete()
        {
            var keyspace = await CreateCassandraKeyspace(null);
            await keyspace.DeleteAsync(WaitUntil.Completed);

            bool exists = await CassandraKeyspaceCollection.ExistsAsync(_keyspaceName);
            Assert.IsFalse(exists);
        }

        internal async Task<CassandraKeyspaceResource> CreateCassandraKeyspace(AutoscaleSettings autoscale)
        {
            _keyspaceName = Recording.GenerateAssetName("cassandra-keyspace-");
            return await CreateCassandraKeyspace(_keyspaceName, autoscale, _keyspaceAccount.GetCassandraKeyspaces());
        }

        internal static async Task<CassandraKeyspaceResource> CreateCassandraKeyspace(string name, AutoscaleSettings autoscale, CassandraKeyspaceCollection collection)
        {
            CassandraKeyspaceCreateUpdateData cassandraKeyspaceCreateUpdateOptions = new CassandraKeyspaceCreateUpdateData(AzureLocation.WestUS,
                new Models.CassandraKeyspaceResource(name))
            {
                Options = BuildDatabaseCreateUpdateOptions(TestThroughput1, autoscale),
            };
            var keyspaceLro = await collection.CreateOrUpdateAsync(WaitUntil.Completed, name, cassandraKeyspaceCreateUpdateOptions);
            return keyspaceLro.Value;
        }

        private void VerifyCassandraKeyspaces(CassandraKeyspaceResource expectedValue, CassandraKeyspaceResource actualValue)
        {
            Assert.AreEqual(expectedValue.Id, actualValue.Id);
            Assert.AreEqual(expectedValue.Data.Name, actualValue.Data.Name);
            Assert.AreEqual(expectedValue.Data.Location, actualValue.Data.Location);
            Assert.AreEqual(expectedValue.Data.Tags, actualValue.Data.Tags);
            Assert.AreEqual(expectedValue.Data.ResourceType, actualValue.Data.ResourceType);

            Assert.AreEqual(expectedValue.Data.Options, actualValue.Data.Options);

            Assert.AreEqual(expectedValue.Data.Resource.Id, actualValue.Data.Resource.Id);
            Assert.AreEqual(expectedValue.Data.Resource.Rid, actualValue.Data.Resource.Rid);
            Assert.AreEqual(expectedValue.Data.Resource.Ts, actualValue.Data.Resource.Ts);
            Assert.AreEqual(expectedValue.Data.Resource.Etag, actualValue.Data.Resource.Etag);
        }
    }
}

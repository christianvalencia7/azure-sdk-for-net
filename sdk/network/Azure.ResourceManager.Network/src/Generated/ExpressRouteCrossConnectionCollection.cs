// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace Azure.ResourceManager.Network
{
    /// <summary>
    /// A class representing a collection of <see cref="ExpressRouteCrossConnectionResource" /> and their operations.
    /// Each <see cref="ExpressRouteCrossConnectionResource" /> in the collection will belong to the same instance of <see cref="ResourceGroupResource" />.
    /// To get an <see cref="ExpressRouteCrossConnectionCollection" /> instance call the GetExpressRouteCrossConnections method from an instance of <see cref="ResourceGroupResource" />.
    /// </summary>
    public partial class ExpressRouteCrossConnectionCollection : ArmCollection, IEnumerable<ExpressRouteCrossConnectionResource>, IAsyncEnumerable<ExpressRouteCrossConnectionResource>
    {
        private readonly ClientDiagnostics _expressRouteCrossConnectionClientDiagnostics;
        private readonly ExpressRouteCrossConnectionsRestOperations _expressRouteCrossConnectionRestClient;

        /// <summary> Initializes a new instance of the <see cref="ExpressRouteCrossConnectionCollection"/> class for mocking. </summary>
        protected ExpressRouteCrossConnectionCollection()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="ExpressRouteCrossConnectionCollection"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the parent resource that is the target of operations. </param>
        internal ExpressRouteCrossConnectionCollection(ArmClient client, ResourceIdentifier id) : base(client, id)
        {
            _expressRouteCrossConnectionClientDiagnostics = new ClientDiagnostics("Azure.ResourceManager.Network", ExpressRouteCrossConnectionResource.ResourceType.Namespace, Diagnostics);
            TryGetApiVersion(ExpressRouteCrossConnectionResource.ResourceType, out string expressRouteCrossConnectionApiVersion);
            _expressRouteCrossConnectionRestClient = new ExpressRouteCrossConnectionsRestOperations(Pipeline, Diagnostics.ApplicationId, Endpoint, expressRouteCrossConnectionApiVersion);
#if DEBUG
			ValidateResourceId(Id);
#endif
        }

        internal static void ValidateResourceId(ResourceIdentifier id)
        {
            if (id.ResourceType != ResourceGroupResource.ResourceType)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource type {0} expected {1}", id.ResourceType, ResourceGroupResource.ResourceType), nameof(id));
        }

        /// <summary>
        /// Update the specified ExpressRouteCrossConnection.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections/{crossConnectionName}
        /// Operation Id: ExpressRouteCrossConnections_CreateOrUpdate
        /// </summary>
        /// <param name="waitUntil"> "F:Azure.WaitUntil.Completed" if the method should wait to return until the long-running operation has completed on the service; "F:Azure.WaitUntil.Started" if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="crossConnectionName"> The name of the ExpressRouteCrossConnection. </param>
        /// <param name="data"> Parameters supplied to the update express route crossConnection operation. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="crossConnectionName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="crossConnectionName"/> or <paramref name="data"/> is null. </exception>
        public virtual async Task<ArmOperation<ExpressRouteCrossConnectionResource>> CreateOrUpdateAsync(WaitUntil waitUntil, string crossConnectionName, ExpressRouteCrossConnectionData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(crossConnectionName, nameof(crossConnectionName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = await _expressRouteCrossConnectionRestClient.CreateOrUpdateAsync(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, data, cancellationToken).ConfigureAwait(false);
                var operation = new NetworkArmOperation<ExpressRouteCrossConnectionResource>(new ExpressRouteCrossConnectionOperationSource(Client), _expressRouteCrossConnectionClientDiagnostics, Pipeline, _expressRouteCrossConnectionRestClient.CreateCreateOrUpdateRequest(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, data).Request, response, OperationFinalStateVia.AzureAsyncOperation);
                if (waitUntil == WaitUntil.Completed)
                    await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Update the specified ExpressRouteCrossConnection.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections/{crossConnectionName}
        /// Operation Id: ExpressRouteCrossConnections_CreateOrUpdate
        /// </summary>
        /// <param name="waitUntil"> "F:Azure.WaitUntil.Completed" if the method should wait to return until the long-running operation has completed on the service; "F:Azure.WaitUntil.Started" if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="crossConnectionName"> The name of the ExpressRouteCrossConnection. </param>
        /// <param name="data"> Parameters supplied to the update express route crossConnection operation. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="crossConnectionName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="crossConnectionName"/> or <paramref name="data"/> is null. </exception>
        public virtual ArmOperation<ExpressRouteCrossConnectionResource> CreateOrUpdate(WaitUntil waitUntil, string crossConnectionName, ExpressRouteCrossConnectionData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(crossConnectionName, nameof(crossConnectionName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = _expressRouteCrossConnectionRestClient.CreateOrUpdate(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, data, cancellationToken);
                var operation = new NetworkArmOperation<ExpressRouteCrossConnectionResource>(new ExpressRouteCrossConnectionOperationSource(Client), _expressRouteCrossConnectionClientDiagnostics, Pipeline, _expressRouteCrossConnectionRestClient.CreateCreateOrUpdateRequest(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, data).Request, response, OperationFinalStateVia.AzureAsyncOperation);
                if (waitUntil == WaitUntil.Completed)
                    operation.WaitForCompletion(cancellationToken);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Gets details about the specified ExpressRouteCrossConnection.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections/{crossConnectionName}
        /// Operation Id: ExpressRouteCrossConnections_Get
        /// </summary>
        /// <param name="crossConnectionName"> The name of the ExpressRouteCrossConnection (service key of the circuit). </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="crossConnectionName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="crossConnectionName"/> is null. </exception>
        public virtual async Task<Response<ExpressRouteCrossConnectionResource>> GetAsync(string crossConnectionName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(crossConnectionName, nameof(crossConnectionName));

            using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.Get");
            scope.Start();
            try
            {
                var response = await _expressRouteCrossConnectionRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new ExpressRouteCrossConnectionResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Gets details about the specified ExpressRouteCrossConnection.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections/{crossConnectionName}
        /// Operation Id: ExpressRouteCrossConnections_Get
        /// </summary>
        /// <param name="crossConnectionName"> The name of the ExpressRouteCrossConnection (service key of the circuit). </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="crossConnectionName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="crossConnectionName"/> is null. </exception>
        public virtual Response<ExpressRouteCrossConnectionResource> Get(string crossConnectionName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(crossConnectionName, nameof(crossConnectionName));

            using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.Get");
            scope.Start();
            try
            {
                var response = _expressRouteCrossConnectionRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, cancellationToken);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new ExpressRouteCrossConnectionResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all the ExpressRouteCrossConnections in a resource group.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections
        /// Operation Id: ExpressRouteCrossConnections_ListByResourceGroup
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="ExpressRouteCrossConnectionResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual AsyncPageable<ExpressRouteCrossConnectionResource> GetAllAsync(CancellationToken cancellationToken = default)
        {
            async Task<Page<ExpressRouteCrossConnectionResource>> FirstPageFunc(int? pageSizeHint)
            {
                using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.GetAll");
                scope.Start();
                try
                {
                    var response = await _expressRouteCrossConnectionRestClient.ListByResourceGroupAsync(Id.SubscriptionId, Id.ResourceGroupName, cancellationToken: cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value.Select(value => new ExpressRouteCrossConnectionResource(Client, value)), response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            async Task<Page<ExpressRouteCrossConnectionResource>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.GetAll");
                scope.Start();
                try
                {
                    var response = await _expressRouteCrossConnectionRestClient.ListByResourceGroupNextPageAsync(nextLink, Id.SubscriptionId, Id.ResourceGroupName, cancellationToken: cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value.Select(value => new ExpressRouteCrossConnectionResource(Client, value)), response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            return PageableHelpers.CreateAsyncEnumerable(FirstPageFunc, NextPageFunc);
        }

        /// <summary>
        /// Retrieves all the ExpressRouteCrossConnections in a resource group.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections
        /// Operation Id: ExpressRouteCrossConnections_ListByResourceGroup
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="ExpressRouteCrossConnectionResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual Pageable<ExpressRouteCrossConnectionResource> GetAll(CancellationToken cancellationToken = default)
        {
            Page<ExpressRouteCrossConnectionResource> FirstPageFunc(int? pageSizeHint)
            {
                using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.GetAll");
                scope.Start();
                try
                {
                    var response = _expressRouteCrossConnectionRestClient.ListByResourceGroup(Id.SubscriptionId, Id.ResourceGroupName, cancellationToken: cancellationToken);
                    return Page.FromValues(response.Value.Value.Select(value => new ExpressRouteCrossConnectionResource(Client, value)), response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            Page<ExpressRouteCrossConnectionResource> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.GetAll");
                scope.Start();
                try
                {
                    var response = _expressRouteCrossConnectionRestClient.ListByResourceGroupNextPage(nextLink, Id.SubscriptionId, Id.ResourceGroupName, cancellationToken: cancellationToken);
                    return Page.FromValues(response.Value.Value.Select(value => new ExpressRouteCrossConnectionResource(Client, value)), response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            return PageableHelpers.CreateEnumerable(FirstPageFunc, NextPageFunc);
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections/{crossConnectionName}
        /// Operation Id: ExpressRouteCrossConnections_Get
        /// </summary>
        /// <param name="crossConnectionName"> The name of the ExpressRouteCrossConnection (service key of the circuit). </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="crossConnectionName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="crossConnectionName"/> is null. </exception>
        public virtual async Task<Response<bool>> ExistsAsync(string crossConnectionName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(crossConnectionName, nameof(crossConnectionName));

            using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.Exists");
            scope.Start();
            try
            {
                var response = await _expressRouteCrossConnectionRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, cancellationToken: cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// Request Path: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/expressRouteCrossConnections/{crossConnectionName}
        /// Operation Id: ExpressRouteCrossConnections_Get
        /// </summary>
        /// <param name="crossConnectionName"> The name of the ExpressRouteCrossConnection (service key of the circuit). </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="crossConnectionName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="crossConnectionName"/> is null. </exception>
        public virtual Response<bool> Exists(string crossConnectionName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(crossConnectionName, nameof(crossConnectionName));

            using var scope = _expressRouteCrossConnectionClientDiagnostics.CreateScope("ExpressRouteCrossConnectionCollection.Exists");
            scope.Start();
            try
            {
                var response = _expressRouteCrossConnectionRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, crossConnectionName, cancellationToken: cancellationToken);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        IEnumerator<ExpressRouteCrossConnectionResource> IEnumerable<ExpressRouteCrossConnectionResource>.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IAsyncEnumerator<ExpressRouteCrossConnectionResource> IAsyncEnumerable<ExpressRouteCrossConnectionResource>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAllAsync(cancellationToken: cancellationToken).GetAsyncEnumerator(cancellationToken);
        }
    }
}

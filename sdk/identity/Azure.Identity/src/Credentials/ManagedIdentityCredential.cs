﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;
using Azure.Core.Diagnostics;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Identitiy;

namespace Azure.Identity
{
    /// <summary>
    /// Attempts authentication using a managed identity that has been assigned to the deployment environment. This authentication type works in Azure VMs,
    /// App Service and Azure Functions applications, as well as the Azure Cloud Shell. More information about configuring managed identities can be found here:
    /// https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/overview
    /// </summary>
    public class ManagedIdentityCredential : TokenCredential
    {
        internal const string MsiUnavailableError = "No managed identity endpoint found.";

        private readonly CredentialPipeline _pipeline;
        private readonly ManagedIdentityClient _client;
        private readonly string _clientId;
        private readonly bool _logAccountDetails;

        private const string Troubleshooting =
            "See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/managedidentitycredential/troubleshoot";

        /// <summary>
        /// Protected constructor for mocking.
        /// </summary>
        protected ManagedIdentityCredential()
        { }

        /// <summary>
        /// Creates an instance of the ManagedIdentityCredential capable of authenticating a resource with a managed identity.
        /// </summary>
        /// <param name="clientId">
        /// The client id to authenticate for a user assigned managed identity.  More information on user assigned managed identities can be found here:
        /// https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/overview#how-a-user-assigned-managed-identity-works-with-an-azure-vm
        /// </param>
        /// <param name="options">Options to configure the management of the requests sent to the Azure Active Directory service.</param>
        public ManagedIdentityCredential(string clientId = null, TokenCredentialOptions options = null)
            : this(clientId, CredentialPipeline.GetInstance(options))
        {
            _logAccountDetails = options?.Diagnostics?.IsAccountIdentifierLoggingEnabled ?? false;
        }

        /// <summary>
        /// Creates an instance of the ManagedIdentityCredential capable of authenticating a resource with a managed identity.
        /// </summary>
        /// <param name="resourceId">
        /// The resource id to authenticate for a user assigned managed identity.  More information on user assigned managed identities can be found here:
        /// https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/overview#how-a-user-assigned-managed-identity-works-with-an-azure-vm
        /// </param>
        /// <param name="options">Options to configure the management of the requests sent to the Azure Active Directory service.</param>
        public ManagedIdentityCredential(ResourceIdentifier resourceId, TokenCredentialOptions options = null)
            : this(
                new ManagedIdentityClient(new ManagedIdentityClientOptions { ResourceIdentifier = resourceId, Pipeline = CredentialPipeline.GetInstance(options) }))
        {
            _logAccountDetails = options?.Diagnostics?.IsAccountIdentifierLoggingEnabled ?? false;
            _clientId = resourceId.ToString();
        }

        internal ManagedIdentityCredential(string clientId, CredentialPipeline pipeline)
            : this(new ManagedIdentityClient(pipeline, clientId))
        {
            _clientId = clientId;
        }

        internal ManagedIdentityCredential(ResourceIdentifier resourceId, CredentialPipeline pipeline, bool preserveTransport = false)
            : this(new ManagedIdentityClient(new ManagedIdentityClientOptions{Pipeline = pipeline, ResourceIdentifier = resourceId, PreserveTransport = preserveTransport}))
        {
            _clientId = resourceId.ToString();
        }

        internal ManagedIdentityCredential(ManagedIdentityClient client)
        {
            _pipeline = client.Pipeline;
            _client = client;
        }

        /// <summary>
        /// Obtains an <see cref="AccessToken"/> from the Managed Identity service if available. This method is called automatically by Azure SDK client libraries. You may call this method directly, but you must also handle token caching and token refreshing.
        /// </summary>
        /// <param name="requestContext">The details of the authentication request.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns>An <see cref="AccessToken"/> which can be used to authenticate service client calls, or a default <see cref="AccessToken"/> if no managed identity is available.</returns>
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            return await GetTokenImplAsync(true, requestContext, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Obtains an <see cref="AccessToken"/> from the Managed Identity service if available. This method is called automatically by Azure SDK client libraries. You may call this method directly, but you must also handle token caching and token refreshing.
        /// </summary>
        /// <param name="requestContext">The details of the authentication request.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns>An <see cref="AccessToken"/> which can be used to authenticate service client calls, or a default <see cref="AccessToken"/> if no managed identity is available.</returns>
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            return GetTokenImplAsync(false, requestContext, cancellationToken).EnsureCompleted();
        }

        private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("ManagedIdentityCredential.GetToken", requestContext);

            try
            {
                AccessToken result = await _client.AuthenticateAsync(async, requestContext, cancellationToken).ConfigureAwait(false);
                if (_logAccountDetails)
                {
                    var accountDetails = TokenHelper.ParseAccountInfoFromToken(result.Token);
                    AzureIdentityEventSource.Singleton.AuthenticatedAccountDetails(accountDetails.ClientId ?? _clientId, accountDetails.TenantId, accountDetails.Upn, accountDetails.ObjectId);
                }
                return scope.Succeeded(result);
            }
            catch (Exception e)
            {
                throw scope.FailWrapAndThrow(e, Troubleshooting);
            }
        }
    }
}

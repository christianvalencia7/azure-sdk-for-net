﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;
using Azure.Core;
using Azure.Core.Diagnostics;

namespace Azure.Identity
{
    [EventSource(Name = EventSourceName)]
    internal sealed class AzureIdentityEventSource : AzureEventSource
    {
        private const string EventSourceName = "Azure-Identity";

        private const int GetTokenEvent = 1;
        private const int GetTokenSucceededEvent = 2;
        private const int GetTokenFailedEvent = 3;
        private const int ProbeImdsEndpointEvent = 4;
        private const int ImdsEndpointFoundEvent = 5;
        private const int ImdsEndpointUnavailableEvent = 6;
        private const int MsalLogVerboseEvent = 7;
        private const int MsalLogInfoEvent = 8;
        private const int MsalLogWarningEvent = 9;
        private const int MsalLogErrorEvent = 10;
        private const int InteractiveAuthenticationThreadPoolExecutionEvent = 11;
        private const int InteractiveAuthenticationInlineExecutionEvent = 12;
        private const int DefaultAzureCredentialCredentialSelectedEvent = 13;
        private const int ProcessRunnerErrorEvent = 14;
        private const int ProcessRunnerInfoEvent = 15;
        private const int UsernamePasswordCredentialAcquireTokenSilentFailedEvent = 16;
        private const int TenantIdDiscoveredAndNotUsedEvent = 17;
        private const int TenantIdDiscoveredAndUsedEvent = 18;
        internal const int AuthenticatedAccountDetailsEvent = 19;
        internal const int UnableToParseAccountDetailsFromTokenEvent = 20;
        internal const string TenantIdDiscoveredAndNotUsedEventMessage = "A token was request for a different tenant than was configured on the credential, but the configured value was used since multi tenant authentication has been disabled. Configured TenantId: {0}, Requested TenantId {1}";
        internal const string TenantIdDiscoveredAndUsedEventMessage = "A token was requested for a different tenant than was configured on the credential, and the requested tenant id was used to authenticate. Configured TenantId: {0}, Requested TenantId {1}";
        internal const string AuthenticatedAccountDetailsMessage = "Client ID: {0}. Tenant ID: {1}. User Principal Name: {2} Object ID: {3}";
        internal const string Unavailable = "<not available>";
        internal const string UnableToParseAccountDetailsFromTokenMessage = "Unable to parse account details from the Access Token";

        private AzureIdentityEventSource() : base(EventSourceName) { }

        public static AzureIdentityEventSource Singleton { get; } = new AzureIdentityEventSource();

        [NonEvent]
        public void GetToken(string method, TokenRequestContext context)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                GetToken(method, FormatStringArray(context.Scopes), context.ParentRequestId);
            }
        }

        [Event(GetTokenEvent, Level = EventLevel.Informational, Message = "{0} invoked. Scopes: {1} ParentRequestId: {2}")]
        public void GetToken(string method, string scopes, string parentRequestId)
        {
            WriteEvent(GetTokenEvent, method, scopes, parentRequestId);
        }

        [NonEvent]
        public void GetTokenSucceeded(string method, TokenRequestContext context, DateTimeOffset expiresOn)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                GetTokenSucceeded(method, FormatStringArray(context.Scopes), context.ParentRequestId, expiresOn.ToString("O", CultureInfo.InvariantCulture));
            }
        }

        [Event(GetTokenSucceededEvent, Level = EventLevel.Informational, Message = "{0} succeeded. Scopes: {1} ParentRequestId: {2} ExpiresOn: {3}")]
        public void GetTokenSucceeded(string method, string scopes, string parentRequestId, string expiresOn)
        {
            WriteEvent(GetTokenSucceededEvent, method, scopes, parentRequestId, expiresOn);
        }

        [NonEvent]
        public void GetTokenFailed(string method, TokenRequestContext context, Exception ex)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                GetTokenFailed(method, FormatStringArray(context.Scopes), context.ParentRequestId, FormatException(ex));
            }
        }

        [Event(GetTokenFailedEvent, Level = EventLevel.Informational, Message = "{0} was unable to retrieve an access token. Scopes: {1} ParentRequestId: {2} Exception: {3}")]
        public void GetTokenFailed(string method, string scopes, string parentRequestId, string exception)
        {
            WriteEvent(GetTokenFailedEvent, method, scopes, parentRequestId, exception);
        }

        [NonEvent]
        public void ProbeImdsEndpoint(Uri uri)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                ProbeImdsEndpoint(uri.AbsoluteUri);
            }
        }

        [Event(ProbeImdsEndpointEvent, Level = EventLevel.Informational, Message = "Probing IMDS endpoint for availability. Endpoint: {0}")]
        public void ProbeImdsEndpoint(string uri)
        {
            WriteEvent(ProbeImdsEndpointEvent, uri);
        }

        [NonEvent]
        public void ImdsEndpointFound(Uri uri)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                ImdsEndpointFound(uri.AbsoluteUri);
            }
        }

        [Event(ImdsEndpointFoundEvent, Level = EventLevel.Informational, Message = "IMDS endpoint is available. Endpoint: {0}")]
        public void ImdsEndpointFound(string uri)
        {
            WriteEvent(ImdsEndpointFoundEvent, uri);
        }

        [NonEvent]
        public void ImdsEndpointUnavailable(Uri uri, string error)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                ImdsEndpointUnavailable(uri.AbsoluteUri, error);
            }
        }

        [NonEvent]
        public void ImdsEndpointUnavailable(Uri uri, Exception e)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                ImdsEndpointUnavailable(uri.AbsoluteUri, FormatException(e));
            }
        }

        [Event(ImdsEndpointUnavailableEvent, Level = EventLevel.Informational, Message = "IMDS endpoint is not available. Endpoint: {0}. Error: {1}")]
        public void ImdsEndpointUnavailable(string uri, string error)
        {
            WriteEvent(ImdsEndpointUnavailableEvent, uri, error);
        }

        [NonEvent]
        private static string FormatException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            bool nest = false;
            do
            {
                if (nest)
                {
                    // Format how Exception.ToString() would.
                    sb.AppendLine()
                        .Append(" ---> ");
                }
                // Do not include StackTrace, but do include HResult (often useful for CryptographicExceptions or IOExceptions).
                sb.Append(ex.GetType().FullName)
                    .Append(" (0x")
                    .Append(ex.HResult.ToString("x", CultureInfo.InvariantCulture))
                    .Append("): ")
                    .Append(ex.Message);
                ex = ex.InnerException;
                nest = true;
            }
            while (ex != null);
            return sb.ToString();
        }

        [NonEvent]
        public void LogMsal(Microsoft.Identity.Client.LogLevel level, string message)
        {
            switch (level)
            {
                case Microsoft.Identity.Client.LogLevel.Error when IsEnabled(EventLevel.Error, EventKeywords.All):
                    LogMsalError(message);
                    break;
                case Microsoft.Identity.Client.LogLevel.Warning when IsEnabled(EventLevel.Warning, EventKeywords.All):
                    LogMsalWarning(message);
                    break;
                case Microsoft.Identity.Client.LogLevel.Info when IsEnabled(EventLevel.Informational, EventKeywords.All):
                    LogMsalInformational(message);
                    break;
                case Microsoft.Identity.Client.LogLevel.Verbose when IsEnabled(EventLevel.Verbose, EventKeywords.All):
                    LogMsalVerbose(message);
                    break;
            }
        }

        [Event(MsalLogErrorEvent, Level = EventLevel.Error, Message = "{0}")]
        public void LogMsalError(string message)
        {
            WriteEvent(MsalLogErrorEvent, message);
        }

        [Event(MsalLogWarningEvent, Level = EventLevel.Warning, Message = "{0}")]
        public void LogMsalWarning(string message)
        {
            WriteEvent(MsalLogWarningEvent, message);
        }

        [Event(MsalLogInfoEvent, Level = EventLevel.Informational, Message = "{0}")]
        public void LogMsalInformational(string message)
        {
            WriteEvent(MsalLogInfoEvent, message);
        }

        [Event(MsalLogVerboseEvent, Level = EventLevel.Verbose, Message = "{0}")]
        public void LogMsalVerbose(string message)
        {
            WriteEvent(MsalLogVerboseEvent, message);
        }

        [NonEvent]
        private static string FormatStringArray(string[] array)
        {
            return new StringBuilder("[ ").Append(string.Join(", ", array)).Append(" ]").ToString();
        }

        [Event(InteractiveAuthenticationThreadPoolExecutionEvent, Level = EventLevel.Informational, Message = "Executing interactive authentication workflow via Task.Run.")]
        public void InteractiveAuthenticationExecutingOnThreadPool()
        {
            WriteEvent(InteractiveAuthenticationThreadPoolExecutionEvent);
        }

        [Event(InteractiveAuthenticationInlineExecutionEvent, Level = EventLevel.Informational, Message = "Executing interactive authentication workflow inline.")]
        public void InteractiveAuthenticationExecutingInline()
        {
            WriteEvent(InteractiveAuthenticationInlineExecutionEvent);
        }

        [Event(DefaultAzureCredentialCredentialSelectedEvent, Level = EventLevel.Informational, Message = "DefaultAzureCredential credential selected: {0}")]
        public void DefaultAzureCredentialCredentialSelected(string credentialType)
        {
            WriteEvent(DefaultAzureCredentialCredentialSelectedEvent, credentialType);
        }

        [NonEvent]
        public void ProcessRunnerError(string message)
        {
            if (IsEnabled(EventLevel.Error, EventKeywords.All))
            {
                LogProcessRunnerError(message);
            }
        }

        [Event(ProcessRunnerErrorEvent, Level = EventLevel.Error, Message = "{0}")]
        public void LogProcessRunnerError(string message)
        {
            WriteEvent(ProcessRunnerErrorEvent, message);
        }

        [NonEvent]
        public void ProcessRunnerInformational(string message)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                LogProcessRunnerInformational(message);
            }
        }

        [Event(ProcessRunnerInfoEvent, Level = EventLevel.Informational, Message = "{0}")]
        public void LogProcessRunnerInformational(string message)
        {
            WriteEvent(ProcessRunnerInfoEvent, message);
        }

        [NonEvent]
        public void UsernamePasswordCredentialAcquireTokenSilentFailed(Exception e)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                UsernamePasswordCredentialAcquireTokenSilentFailed(FormatException(e));
            }
        }

        [Event(
            UsernamePasswordCredentialAcquireTokenSilentFailedEvent,
            Level = EventLevel.Informational,
            Message = "UsernamePasswordCredential failed to acquire token silently. Error: {1}")]
        public void UsernamePasswordCredentialAcquireTokenSilentFailed(string error)
        {
            WriteEvent(UsernamePasswordCredentialAcquireTokenSilentFailedEvent, error);
        }

        [Event(TenantIdDiscoveredAndNotUsedEvent, Level = EventLevel.Informational, Message = TenantIdDiscoveredAndNotUsedEventMessage)]
        public void TenantIdDiscoveredAndNotUsed(string explicitTenantId, string contextTenantId)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                WriteEvent(TenantIdDiscoveredAndNotUsedEvent, explicitTenantId, contextTenantId);
            }
        }

        [Event(TenantIdDiscoveredAndUsedEvent, Level = EventLevel.Informational, Message = TenantIdDiscoveredAndUsedEventMessage)]
        public void TenantIdDiscoveredAndUsed(string explicitTenantId, string contextTenantId)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                WriteEvent(TenantIdDiscoveredAndUsedEvent, explicitTenantId, contextTenantId);
            }
        }

        [Event(AuthenticatedAccountDetailsEvent, Level = EventLevel.Informational, Message = AuthenticatedAccountDetailsMessage)]
        public void AuthenticatedAccountDetails(string clientId, string tenantId, string upn, string objectId)
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                WriteEvent(AuthenticatedAccountDetailsEvent, clientId ?? Unavailable, tenantId ?? Unavailable, upn ?? Unavailable, objectId ?? Unavailable);
            }
        }

        [Event(UnableToParseAccountDetailsFromTokenEvent, Level = EventLevel.Informational, Message = UnableToParseAccountDetailsFromTokenMessage)]
        internal void UnableToParseAccountDetailsFromToken()
        {
            if (IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                WriteEvent(UnableToParseAccountDetailsFromTokenEvent);
            }
        }
    }
}

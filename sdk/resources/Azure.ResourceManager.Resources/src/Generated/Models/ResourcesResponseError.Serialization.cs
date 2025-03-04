// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Resources.Models
{
    public partial class ResourcesResponseError
    {
        internal static ResourcesResponseError DeserializeResourcesResponseError(JsonElement element)
        {
            Optional<string> code = default;
            Optional<string> message = default;
            Optional<string> target = default;
            Optional<IReadOnlyList<ResourcesResponseError>> details = default;
            Optional<IReadOnlyList<ResourcesErrorAdditionalInfo>> additionalInfo = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("code"))
                {
                    code = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("message"))
                {
                    message = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("target"))
                {
                    target = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("details"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<ResourcesResponseError> array = new List<ResourcesResponseError>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(DeserializeResourcesResponseError(item));
                    }
                    details = array;
                    continue;
                }
                if (property.NameEquals("additionalInfo"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<ResourcesErrorAdditionalInfo> array = new List<ResourcesErrorAdditionalInfo>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(ResourcesErrorAdditionalInfo.DeserializeResourcesErrorAdditionalInfo(item));
                    }
                    additionalInfo = array;
                    continue;
                }
            }
            return new ResourcesResponseError(code.Value, message.Value, target.Value, Optional.ToList(details), Optional.ToList(additionalInfo));
        }
    }
}

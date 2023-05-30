// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Drastic.Tempest;

namespace VisualTestUtils.AppConnector.Messages;

public class ClientRegistrationMessage : AppConnectorMessage
{
    public ClientRegistrationMessage()
        : base(AppConnectorMessageType.ClientRegistration)
    {
    }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        base.WritePayload(context, writer);
    }
}

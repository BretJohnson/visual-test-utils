using Drastic.Tempest;

namespace VisualTestUtils.AppConnector.Messages;

public class TestRequestMessage : AppConnectorMessage
{
    public TestRequestMessage()
        : base(AppConnectorMessageType.TestRequest)
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

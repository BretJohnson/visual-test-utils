using Drastic.Tempest;
using System.Text;

namespace VisualTestUtils.AppConnector.Messages;

public class PingControllerRequest : AppConnectorMessage
{
    public string SenderName { get; internal set; }

    public PingControllerRequest(string senderName = "")
        : base(AppConnectorMessageType.PingAppRequest)
    {
        this.SenderName = senderName;
    }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.SenderName = reader.ReadString(Encoding.UTF8);
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteString(Encoding.UTF8, this.SenderName);
        base.WritePayload(context, writer);
    }
}

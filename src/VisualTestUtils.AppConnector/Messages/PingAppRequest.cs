using System.Text;
using Drastic.Tempest;

namespace VisualTestUtils.AppConnector.Messages;

public class PingAppRequest : AppConnectorMessage
{
    public string SenderName { get; internal set; } = string.Empty;

    public PingAppRequest()
        : base(AppConnectorMessageType.PingAppRequest)
    {
    }

    public override string ToString() =>
        $"{base.ToString()} - {this.SenderName}";

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

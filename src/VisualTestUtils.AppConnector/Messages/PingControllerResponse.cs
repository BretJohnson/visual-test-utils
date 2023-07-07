using Drastic.Tempest;
using System.Text;

namespace VisualTestUtils.AppConnector.Messages;

public class PingControllerResponse : AppConnectorMessage
{
    public string HelloMessage { get; internal set; } = string.Empty;

    public PingControllerResponse()
        : base(AppConnectorMessageType.PingControllerResponse)
    {
    }

    public override string ToString() =>
        $"{base.ToString()} - {this.HelloMessage}";

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.HelloMessage = reader.ReadString(Encoding.UTF8);
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteString(Encoding.UTF8, this.HelloMessage);
        base.WritePayload(context, writer);
    }
}

using System.Text;
using Drastic.Tempest;

namespace VisualTestUtils.AppConnector.Messages;

public class InvokeAppMethodResponse : AppConnectorMessage
{
    public string[] ReturnValue { get; internal set; } = Array.Empty<string>();

    public InvokeAppMethodResponse()
        : base(AppConnectorMessageType.InvokeAppMethodResponse)
    {
    }

    public override string ToString() =>
        $"{base.ToString()} - {string.Join(",", this.ReturnValue)})";

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.ReturnValue = reader.Read<string[]>(context);
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.Write(context, this.ReturnValue);
        base.WritePayload(context, writer);
    }
}

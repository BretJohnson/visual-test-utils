using System.Text;
using Drastic.Tempest;

namespace VisualTestUtils.AppConnector.Messages;

public class InvokeAppMethodRequest : AppConnectorMessage
{
    public string FullyQualifiedTypeName { get; internal set; } = string.Empty;
    public string StaticMethodName { get; internal set; } = string.Empty;
    public string[] Args { get; internal set; } = Array.Empty<string>();

    public InvokeAppMethodRequest()
        : base(AppConnectorMessageType.InvokeAppMethodRequest)
    {
    }

    public override string ToString() =>
        $"{base.ToString()} - {this.FullyQualifiedTypeName}.{this.StaticMethodName}({string.Join(",", this.Args)})";

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.FullyQualifiedTypeName = reader.ReadString(Encoding.UTF8);
        this.StaticMethodName = reader.ReadString(Encoding.UTF8);
        this.Args = reader.Read<string[]>(context);
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteString(Encoding.UTF8, this.FullyQualifiedTypeName);
        writer.WriteString(Encoding.UTF8, this.StaticMethodName);
        writer.Write(context, this.Args);
        base.WritePayload(context, writer);
    }
}

using Drastic.Tempest;

namespace VisualTestUtils.AppConnector.Messages;

public abstract class AppConnectorMessage : Message
{
    protected AppConnectorMessage(AppConnectorMessageType type)
        : base(AppConnectorProtocol.Instance, (ushort)type)
    {
        this.Id = "Unknown";
    }

    public string Id { get; internal set; }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.Id = reader.ReadString();
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteString(this.Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.Id}: {this.GetType().ToString()}";
    }
}

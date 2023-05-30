using Drastic.Tempest;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector.Messages;

namespace VisualTestUtils.AppConnector;

public class AppConnectorServer : TempestServer
{
    private readonly ILogger? logger;
    private readonly List<IConnection> connections = new List<IConnection>();

    public AppConnectorServer(IConnectionProvider provider, ILogger? logger = default)
        : base(provider, MessageTypes.Reliable)
    {
        this.logger = logger;

        this.RegisterMessageHandler<TestRequestMessage>(this.OnTestRequestMessage);
        this.RegisterMessageHandler<TestResponseMessage>(this.OnTestResponseMessage);
        this.RegisterMessageHandler<ClientRegistrationMessage>(this.OnClientRegistration);
    }

#if LATER
    public void SendScreenshotRequest()
    {
        this.SendToAll(new OnScreenshotRequestMessage());
    }

    private void OnScreenshotResponse(MessageEventArgs<OnScreenshotResponseMessage> obj)
    {
        foreach (var item in obj.Message.ScreenShots)
        {
            var outputName = !string.IsNullOrEmpty(item.Name) ? item.Name : obj.Message.Id;
            var filename = $"{outputName}.png";
            var output = Path.Combine(this.outputDirectory, filename);
            File.WriteAllBytes(output, item.Image);
            this.logger?.LogInformation($"Saved {output}");
        }
    }
#endif

    /// <inheritdoc/>
    protected override void OnConnectionMade(object sender, ConnectionMadeEventArgs e)
    {
        lock (this.connections)
        {
            this.connections.Add(e.Connection);
        }

        this.logger?.LogInformation(e.ToString());
        base.OnConnectionMade(sender, e);
    }

    /// <inheritdoc/>
    protected override void OnConnectionDisconnected(object sender, DisconnectedEventArgs e)
    {
        lock (this.connections)
        {
            this.connections.Remove(e.Connection);
        }

        this.logger?.LogInformation("Disconnect");
        base.OnConnectionDisconnected(sender, e);
    }

    private void OnTestResponseMessage(MessageEventArgs<TestResponseMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());

        this.SendToAll(args.Message, args.Connection);
    }

    private void OnTestRequestMessage(MessageEventArgs<TestRequestMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());

        this.SendToAll(args.Message, args.Connection);
    }

    private void SendToAll(AppConnectorMessage message, IConnection? ogSender = default)
    {
        lock (this.connections)
        {
            IEnumerable<IConnection> list = this.connections.Where(n => n != ogSender);
            foreach (IConnection connection in list) {
                connection.SendAsync(message);
            }
        }
    }

    private void OnClientRegistration(MessageEventArgs<ClientRegistrationMessage> args)
    {
        ClientRegistrationMessage clientMessage = args.Message;
        this.logger?.LogInformation("Client ID {cliendId} registered", clientMessage.Id);
    }
}

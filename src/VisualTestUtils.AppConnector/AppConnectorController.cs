using Drastic.Tempest;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector.Messages;

namespace VisualTestUtils.AppConnector;

public class AppConnectorController : TempestServer
{
    private readonly ILogger? logger;
    private readonly List<IConnection> connections = new List<IConnection>();

    public AppConnectorController(IConnectionProvider provider, ILogger? logger = default)
        : base(provider, MessageTypes.Reliable)
    {
        this.logger = logger;

        this.RegisterMessageHandler<PingAppResponse>(this.OnPingClientResponse);
        this.RegisterMessageHandler<AppRegistrationRequest>(this.OnClientRegistration);
    }

    public void SendPingClientRequest(string senderName)
    {
        this.SendToAll(new PingAppRequest()
        {
             SenderName = senderName
        });
    }

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

    private void OnPingClientResponse(MessageEventArgs<PingAppResponse> args)
    {
        this.logger?.LogInformation(args.Message.ToString());
        this.SendToAll(args.Message, args.Connection);
    }

    private void SendToAll(AppConnectorMessage message, IConnection? ogSender = default)
    {
        lock (this.connections)
        {
            IEnumerable<IConnection> list = this.connections.Where(n => n != ogSender);
            foreach (IConnection connection in list)
            {
                connection.SendAsync(message);
            }
        }
    }

    private void OnClientRegistration(MessageEventArgs<AppRegistrationRequest> args)
    {
        AppRegistrationRequest clientMessage = args.Message;
        this.logger?.LogInformation("Client ID {cliendId} registered", clientMessage.Id);
    }
}

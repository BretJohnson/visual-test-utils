using Drastic.Tempest;
using Drastic.Tempest.Providers.Network;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector.Messages;

namespace VisualTestUtils.AppConnector;

public class AppConnectorClient : TempestClient
{
    private readonly ILogger? logger;
    private Guid id;
    private string name;

    public AppConnectorClient(string name = "", ILogger? logger = default)
        : this(AppConnectorProtocol.Instance, name, logger)
    {
        this.Connected += this.AppClient_Connected;
    }

    public AppConnectorClient(Protocol protocol, string name = "", ILogger? logger = default)
        : base(new NetworkClientConnection(protocol), MessageTypes.Reliable)
    {
        this.name = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;
        this.id = Guid.NewGuid();
        this.logger = logger;
        this.Connected += this.DiagnosticsClient_Connected;
        this.Disconnected += this.DiagnosticsClient_Disconnected;
        this.RegisterMessageHandler<TestRequestMessage>(this.OnTestRequest);
        this.RegisterMessageHandler<TestResponseMessage>(this.OnTestResponse);
    }

    public string Id => $"{this.name}-{this.id}";

    internal ILogger? Logger => this.logger;

    public Task SendMessageAsync(AppConnectorMessage message)
    {
        message.Id = this.Id;
        return this.Connection.SendAsync(message);
    }

    private void OnTestResponse(MessageEventArgs<TestResponseMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());
    }

    private void OnTestRequest(MessageEventArgs<TestRequestMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());
        this.SendMessageAsync(new TestResponseMessage());
    }

    private void DiagnosticsClient_Disconnected(object? sender, ClientDisconnectedEventArgs e)
    {
        this.logger?.LogInformation($"Disconnect: {e.Reason}");
    }

    private void DiagnosticsClient_Connected(object? sender, ClientConnectionEventArgs e)
    {
        this.logger?.LogInformation($"Connect");
    }

    private void AppClient_Connected(object? sender, ClientConnectionEventArgs e)
    {
        // Tell the server our ID.
        this.SendMessageAsync(new ClientRegistrationMessage());
    }
}

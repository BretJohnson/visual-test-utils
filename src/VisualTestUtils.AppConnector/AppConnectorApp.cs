using System.Reflection;
using Drastic.Tempest;
using Drastic.Tempest.Providers.Network;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector.Messages;

namespace VisualTestUtils.AppConnector;

public class AppConnectorApp : TempestClient
{
    private readonly ILogger? logger;
    private Guid id;
    private string name;

    public AppConnectorApp(string name = "", ILogger? logger = default)
        : this(AppConnectorProtocol.Instance, name, logger)
    {
        this.Connected += this.AppClient_Connected;
    }

    public AppConnectorApp(Protocol protocol, string name = "", ILogger? logger = default,
        bool supportInvokeAppMethod = false)
        : base(new NetworkClientConnection(protocol), MessageTypes.Reliable)
    {
        this.name = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;
        this.id = Guid.NewGuid();
        this.logger = logger;
        this.Connected += this.DiagnosticsClient_Connected;
        this.Disconnected += this.DiagnosticsClient_Disconnected;
        this.RegisterMessageHandler<PingAppRequest>(this.OnPingClientRequest);

        // For security reasons, don't support invoke app method unless explicitly enabled.
        if (supportInvokeAppMethod)
        {
            this.RegisterMessageHandler<InvokeAppMethodRequest>(this.OnInvokeAppMethodRequest);
        }
    }

    public string Id => $"{this.name}-{this.id}";

    internal ILogger? Logger => this.logger;

    public Task SendMessageAsync(AppConnectorMessage message)
    {
        message.Id = this.Id;
        return this.Connection.SendAsync(message);
    }

    private void OnPingClientRequest(MessageEventArgs<PingAppRequest> args)
    {
        this.logger?.LogInformation(args.Message.ToString());
        this.SendMessageAsync(new PingAppResponse() {  HelloMessage = $"Hello {args.Message.SenderName}"});
    }

    private void OnInvokeAppMethodRequest(MessageEventArgs<InvokeAppMethodRequest> args)
    {
        this.logger?.LogInformation(args.Message.ToString());

        Type type = Type.GetType(args.Message.FullyQualifiedTypeName);
        MethodInfo? method = type?.GetMethod(args.Message.StaticMethodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
        object? result = method?.Invoke(null, args.Message.Args);
        this.SendMessageAsync(new InvokeAppMethodResponse() { ReturnValue = result as string[] ?? new string[0] });
    }

    private void DiagnosticsClient_Disconnected(object? sender, ClientDisconnectedEventArgs e)
    {
        this.logger?.LogInformation("Disconnect: {0}", e.Reason);
    }

    private void DiagnosticsClient_Connected(object? sender, ClientConnectionEventArgs e)
    {
        this.logger?.LogInformation("Connect");
    }

    private void AppClient_Connected(object? sender, ClientConnectionEventArgs e)
    {
        // Tell the server our ID.
        this.SendMessageAsync(new AppRegistrationRequest());
    }
}

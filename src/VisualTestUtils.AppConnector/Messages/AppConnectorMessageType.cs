namespace VisualTestUtils.AppConnector.Messages;

public enum AppConnectorMessageType : ushort
{
    ClientRegistration = 1,
    AppClientDiscoveryResponse = 2,
    AppClientConnect = 3,
    AppClientDisconnect = 4,
    PingControllerRequest = 5,
    PingControllerResponse = 6,
    PingAppRequest = 7,
    PingAppResponse = 8,
    InvokeAppMethodRequest = 9,
    InvokeAppMethodResponse = 10
}

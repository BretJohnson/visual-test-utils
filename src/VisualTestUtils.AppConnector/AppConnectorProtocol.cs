using Drastic.Tempest;

namespace VisualTestUtils.AppConnector;

public static class AppConnectorProtocol
{
    public static readonly Protocol Instance = new Protocol(43, 1);

    static AppConnectorProtocol()
    {
        Instance.Discover();
    }
}

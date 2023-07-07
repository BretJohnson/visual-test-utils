using Drastic.Tempest;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector;

namespace SandboxServer;

public class SandboxController : AppConnectorController
{
    private readonly ILogger? logger;

    public SandboxController(IConnectionProvider provider, ILogger? logger = default)
        : base(provider, logger)
    {
        this.logger = logger;
    }
}

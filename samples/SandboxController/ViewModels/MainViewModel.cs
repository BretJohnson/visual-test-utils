// <copyright file="MainViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Drastic.Tempest;
using Drastic.Tempest.Providers.Network;
using Drastic.Tools;
using Drastic.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector;

namespace SandboxServer.ViewModels;

/// <summary>
    /// Main View Model.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private System.Net.NetworkInformation.NetworkInterface? selectedInterface;
        private SandboxController? server;
        private int? port = 8888;
        private ILogger? logger;

        private bool isValidPort => this.port is not null && (this.port > 0 && this.port <= 65535);

        public bool IsServerRunning => this.server?.IsRunning ?? false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="services">Services.</param>
        public MainViewModel(IServiceProvider services)
            : base(services)
        {
            ILoggerProvider? loggerFactory = services.GetService<ILoggerProvider>();
            if (loggerFactory is not null)
            {
                this.logger = loggerFactory.CreateLogger("Server");
            }

            IEnumerable<System.Net.NetworkInformation.NetworkInterface> test = NetworkUtils.GoodInterfaces();
            this.NetworkInterfaces = new ObservableCollection<System.Net.NetworkInformation.NetworkInterface>();
            this.StartServerCommand = new AsyncCommand<NetworkInterface>(this.StartServerAsync, (net) =>
                net is not null && this.isValidPort && !this.IsServerRunning,
                this.ErrorHandler);

            this.StopServerCommand = new AsyncCommand(this.StopServerAsync, () => this.IsServerRunning, this.Dispatcher, this.ErrorHandler);
        }

        public AsyncCommand StopServerCommand { get; }

        public AsyncCommand<NetworkInterface> StartServerCommand { get; }

        public ObservableCollection<System.Net.NetworkInformation.NetworkInterface> NetworkInterfaces { get; }

        public string IPAddress
        {
            get
            {
                if (this.selectedInterface is not null)
                {
                    return this.selectedInterface.GetIPProperties().UnicastAddresses.Where(y => y.Address.AddressFamily == AddressFamily.InterNetwork)
                             .Select(y => y.Address.ToString()).First();
                }

                return string.Empty;
            }
        }

        // Write a function to tell a joke

        /// <summary>
        /// Gets or sets the selected port.
        /// </summary>
        public int? Port
        {
            get
            {
                return this.port;
            }

            set
            {
                this.SetProperty(ref this.port, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected interface.
        /// </summary>
        public System.Net.NetworkInformation.NetworkInterface? SelectedInterface
        {
            get
            {
                return this.selectedInterface;
            }

            set
            {
                this.SetProperty(ref this.selectedInterface, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();
            await this.Reload();
        }

        public async Task Reload()
        {
            this.NetworkInterfaces.Clear();

            foreach (NetworkInterface item in NetworkUtils.GoodInterfaces())
            {
                this.NetworkInterfaces.Add(item);
            }

            this.SelectedInterface = this.NetworkInterfaces.LastOrDefault();
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.StartServerCommand.RaiseCanExecuteChanged();
            this.StopServerCommand.RaiseCanExecuteChanged();
            this.OnPropertyChanged(nameof(this.IPAddress));
            this.OnPropertyChanged(nameof(this.IsServerRunning));
            base.RaiseCanExecuteChanged();
        }

        public void SendPingClientRequest(string senderName)
        {
            if (!this.IsServerRunning)
            {
                return;
            }

            this.server?.SendPingClientRequest(senderName);
        }

        private async Task StartServerAsync(NetworkInterface netInterface)
        {
            if (this.server is not null)
            {
                // TODO: Dispose.
                return;
            }

            var target = new Target(this.IPAddress, this.port ?? 8888);
            var provider = new NetworkConnectionProvider(AppConnectorProtocol.Instance, target, 100);
            this.server = new SandboxController(provider, this.logger);
            this.logger?.LogInformation($"Server Started: {target}");
            this.server.Start();
            this.RaiseCanExecuteChanged();
        }

        private async Task StopServerAsync()
        {
            if (this.server is null)
            {
                return;
            }

            this.logger?.LogInformation($"Server Stopped");
            this.server.Stop();
            this.server = null;
            this.RaiseCanExecuteChanged();
        }
    }

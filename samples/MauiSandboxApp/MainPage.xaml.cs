using Drastic.Tempest;
using VisualTestUtils.AppConnector;

namespace MauiSandboxApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            this.InitializeComponent();

            var client = new AppConnectorApp("TestClient");
            // 8888 = port number
            // Async Command, can be fired off on any thread.
            client.ConnectAsync(new Target("10.0.0.71", 8888));
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            this.count++;

            if (this.count == 1)
                this.CounterBtn.Text = $"Clicked {this.count} time";
            else
                this.CounterBtn.Text = $"Clicked {this.count} times";

            SemanticScreenReader.Announce(this.CounterBtn.Text);
        }
    }
}

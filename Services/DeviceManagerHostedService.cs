using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace D2C.Services.Hosted
{
    public class DeviceManagerHostedService : BackgroundService, IDeviceClientsManager
    {
        readonly IConfiguration configuration;

        IDictionary<string, DeviceClient> deviceClients = null;

        protected IServiceProvider Services { get; }

        public DeviceManagerHostedService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await this.ShutDown();
            await Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.InitiateConnectionsAsync(stoppingToken);
        }

        #region IDeviceClientsManager

        public async Task SendMessageAsync(string deviceId, string message)
        {
            var deviceClient = this.deviceClients[deviceId];
            if (deviceClient != null)
            {
                using (var eventMsg = new Message(Encoding.UTF8.GetBytes(message)))
                {
                    await deviceClient.SendEventAsync(eventMsg);
                }
            }
        }

        public Task InitiateConnectionsAsync(CancellationToken cancelToken)
        {
            try
            {
                deviceClients = SetupDeviceClients(cancelToken).GetAwaiter().GetResult();
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                ShutDown();
                return Task.FromException(e);
            }
        }

        private async Task<Dictionary<string, DeviceClient>> SetupDeviceClients(CancellationToken cancelToken)
        {
            Dictionary<string, DeviceClient> deviceClients = null;
            var deviceconnectionStrings = this.configuration.GetSection("Devices:Scefs").Get<List<string>>();
            if (deviceconnectionStrings != null)
            {
                deviceClients = new Dictionary<string, DeviceClient>();
                foreach (string dcs in deviceconnectionStrings)
                {
                    var tokens = dcs.Split(';');
                    var subTokens = tokens[1].Split('=');
                    var devId = subTokens[1];
                    deviceClients[devId] = DeviceClient.CreateFromConnectionString(dcs, TransportType.Mqtt_Tcp_Only);
                    await deviceClients[devId].OpenAsync(cancelToken);
                }
            }
            return deviceClients;
        }

        public Task ShutDown()
        {
            if (deviceClients != null)
            {
                foreach (var deviceClientPair in deviceClients)
                {
                    if (deviceClientPair.Value != null)
                    {
                        deviceClientPair.Value.CloseAsync().GetAwaiter().GetResult();
                        deviceClientPair.Value.Dispose();
                    }
                }
            }
            return Task.FromResult(true);
        }

        #endregion
    }
}
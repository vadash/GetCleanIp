using System.Net.NetworkInformation;
using GetCleanIp.Interfaces;

namespace GetCleanIp.Services;

public class NetworkAnalyzer : INetworkAnalyzer
{
    public async Task<Dictionary<string, double>> PingAddressesAsync(IEnumerable<string> ipAddresses, int attempts = 3)
    {
        var results = new Dictionary<string, double>();
        var ping = new Ping();

        foreach (var ip in ipAddresses)
        {
            var successfulPings = new List<long>();

            for (var i = 0; i < attempts; i++)
            {
                try
                {
                    var reply = await ping.SendPingAsync(ip, 1000);
                    if (reply.Status == IPStatus.Success)
                    {
                        successfulPings.Add(reply.RoundtripTime);
                    }
                }
                catch (PingException)
                {
                    // Log ping failure
                    continue;
                }
            }

            if (successfulPings.Any())
            {
                results[ip] = successfulPings.Average();
            }
        }

        return results;
    }
}
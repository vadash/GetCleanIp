namespace GetCleanIp.Interfaces;

public interface INetworkAnalyzer
{
    Task<Dictionary<string, double>> PingAddressesAsync(IEnumerable<string> ipAddresses, int attempts = 5);
}
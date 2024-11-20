using GetCleanIp.Interfaces;

namespace GetCleanIp.Services;

public class IpProcessor : IIpProcessor
{
    private readonly Random _random = new Random();

    public Dictionary<string, List<string>> GroupBySubnet(IEnumerable<string> ipAddresses)
    {
        return ipAddresses
            .GroupBy(ip => string.Join(".", ip.Split('.').Take(1)))
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public Dictionary<string, List<string>> LimitGroupSize(Dictionary<string, List<string>> groups, int maxSize = 5)
    {
        var result = new Dictionary<string, List<string>>();
        
        foreach (var group in groups)
        {
            var ips = group.Value;
            if (ips.Count > maxSize)
            {
                ips = ips.OrderBy(x => _random.Next()).Take(maxSize).ToList();
            }
            result[group.Key] = ips;
        }

        return result;
    }

    public Dictionary<string, List<string>> FilterByThresholds(
        Dictionary<string, List<string>> groups,
        Dictionary<string, double> pingResults,
        int threshold1,
        int threshold2)
    {
        var result = new Dictionary<string, List<string>>();

        foreach (var group in groups)
        {
            var filteredIps = group.Value
                .Where(ip => pingResults.ContainsKey(ip) &&
                             pingResults[ip] >= threshold1 &&
                             pingResults[ip] <= threshold2)
                .ToList();

            if (filteredIps.Any())
            {
                result[group.Key] = filteredIps;
            }
        }

        return result;
    }

    public Dictionary<string, string> SelectRandomIps(Dictionary<string, List<string>> groups)
    {
        return groups.ToDictionary(
            g => g.Key,
            g => g.Value[_random.Next(g.Value.Count)]
        );
    }
}
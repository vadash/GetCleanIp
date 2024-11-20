using System.Net;
using System.Text.Json;
using GetCleanIp.Interfaces;

namespace GetCleanIp.Services;

public class IpExtractor : IIpExtractor
{
    public IEnumerable<string> ExtractIpAddresses(string jsonFilePath)
    {
        var jsonContent = File.ReadAllText(jsonFilePath);
        var ipAddresses = new HashSet<string>();
            
        using (var document = JsonDocument.Parse(jsonContent))
        {
            ExtractIpsRecursive(document.RootElement, ipAddresses);
        }

        return ipAddresses.Where(ip => IPAddress.TryParse(ip, out _));
    }

    private static void ExtractIpsRecursive(JsonElement element, HashSet<string> ipAddresses)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                var value = element.GetString();
                if (value != null && IPAddress.TryParse(value, out _))
                    ipAddresses.Add(value);
                break;

            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                    ExtractIpsRecursive(property.Value, ipAddresses);
                break;

            case JsonValueKind.Array:
                foreach (var item in element.EnumerateArray())
                    ExtractIpsRecursive(item, ipAddresses);
                break;
        }
    }
}
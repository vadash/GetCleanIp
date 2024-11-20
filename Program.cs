using System.Text.Json;
using GetCleanIp.Models;
using GetCleanIp.Services;

namespace GetCleanIp;

public static class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Load configuration
            var config = LoadConfiguration();

            // Initialize services
            var fileLocator = new FileLocator();
            var ipExtractor = new IpExtractor();
            var networkAnalyzer = new NetworkAnalyzer();
            var ipProcessor = new IpProcessor();

            // Get latest JSON file
            var jsonFile = fileLocator.GetLatestJsonFile(config.SearchDirectory);
            Console.WriteLine($"Processing file: {jsonFile}");

            // Extract IP addresses
            var ipAddresses = ipExtractor.ExtractIpAddresses(jsonFile);
            Console.WriteLine($"Found {ipAddresses.Count()} IP addresses");

            // Group by subnet
            var groups = ipProcessor.GroupBySubnet(ipAddresses);
            Console.WriteLine($"Grouped into {groups.Count} subnets");

            // Limit group sizes
            groups = ipProcessor.LimitGroupSize(groups);

            // Ping addresses
            Console.WriteLine("Pinging IP addresses...");
            var pingResults = await networkAnalyzer.PingAddressesAsync(
                groups.SelectMany(g => g.Value));

            // Filter by thresholds
            groups = ipProcessor.FilterByThresholds(
                groups, pingResults, config.Threshold1, config.Threshold2);

            // Select random IPs
            var selectedIps = ipProcessor.SelectRandomIps(groups);

            // Write output
            var output = string.Join(",", selectedIps.Values);
            File.WriteAllText("output.txt", output);
            Console.WriteLine($"Selected IPs written to output.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static Config LoadConfiguration()
    {
        var jsonConfig = File.ReadAllText("config.json");
        return JsonSerializer.Deserialize<Config>(jsonConfig) 
               ?? throw new InvalidOperationException("Failed to load configuration");
    }
}
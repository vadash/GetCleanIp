namespace GetCleanIp.Interfaces;

public interface IIpProcessor
{
    Dictionary<string, List<string>> GroupBySubnet(IEnumerable<string> ipAddresses);
    Dictionary<string, List<string>> LimitGroupSize(Dictionary<string, List<string>> groups, int maxSize = 20);
    Dictionary<string, List<string>> FilterByThresholds(Dictionary<string, List<string>> groups, Dictionary<string, double> pingResults, int threshold1, int threshold2);
    Dictionary<string, string> SelectRandomIps(Dictionary<string, List<string>> groups);
}
namespace GetCleanIp.Interfaces;

public interface IIpExtractor
{
    IEnumerable<string> ExtractIpAddresses(string jsonFilePath);
}
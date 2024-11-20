namespace GetCleanIp.Interfaces;

public interface IFileLocator
{
    string GetLatestJsonFile(string directory);
}
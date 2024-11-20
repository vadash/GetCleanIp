using GetCleanIp.Interfaces;

namespace GetCleanIp.Services;

public class FileLocator : IFileLocator
{
    public string GetLatestJsonFile(string directory)
    {
        var jsonFiles = Directory.GetFiles(directory, "*.json");
        if (!jsonFiles.Any()) throw new FileNotFoundException("No JSON files found in the specified directory.");

        return jsonFiles
            .Select(f => new FileInfo(f))
            .OrderByDescending(f => f.LastWriteTime)
            .First()
            .FullName;
    }
}
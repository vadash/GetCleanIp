namespace GetCleanIp.Models;

public class Config
{
    public int Threshold1 { get; set; } = 50;
    public int Threshold2 { get; set; } = 100;
    public string SearchDirectory { get; set; } = @"C:\portable\WinCFScan\results";
}

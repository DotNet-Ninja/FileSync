namespace FileSync.Configuration;

public class SyncTarget
{
    public string Name { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
}
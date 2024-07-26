using System.IO.Abstractions;
using System.Text.Json;

namespace FileSync.Configuration;

public class Settings
{
    private readonly IFileSystem _fileSystem;

    public Settings(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public static string FilePath => 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".filesync", "settings.json");

    public List<SyncTarget> Targets { get; set; } = new();

    public string ReportPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".filesync", "report.json");

    public string ErrorPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "filesync.errors.json");

    public int MaxSyncMinutes { get; set; } = 60;

    public Task SaveAsync()
    {   
        var data = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        var file = _fileSystem.FileInfo.New(FilePath);
        if (!file.Directory?.Exists??false)
        {
            file.Directory.Create();
        }

        return _fileSystem.File.WriteAllTextAsync(FilePath, data);
    }
}
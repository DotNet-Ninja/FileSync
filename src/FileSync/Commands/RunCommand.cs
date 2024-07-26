using System.Diagnostics;
using System.IO.Abstractions;
using System.Text.Json;
using FileSync.Configuration;
using FileSync.Constants;
using FileSync.Model;
using FileSync.Options;
using FileSync.Services;

namespace FileSync.Commands;

public class RunCommand: Command<RunOptions>
{
    private readonly Settings _settings;
    private readonly IFileSystem _fileSystem;

    public RunCommand(IConsole console, Settings settings, IFileSystem fileSystem) : base(console)
    {
        _settings = settings;
        _fileSystem = fileSystem;
    }

    protected override Task<int> ExecuteCommandAsync(RunOptions options)
    {
        var results = new RunResults();
        var targets = options.Targets.Any()
            ? _settings.Targets.Where(t => options.Targets.Contains(t.Name)).ToList()
            : _settings.Targets;
        foreach(var target in targets)
        {
            var result = SyncTarget(target);
            Console.WriteLine($"{result.FileCount} files copied in {result.Duration} with {result.Errors.Count} errors");
            results.Results.Add(result);
        }

        try
        {
            var data = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
            var file = _fileSystem.FileInfo.New(_settings.ReportPath);
            if(!file.Directory?.Exists??false)
            {
                file.Directory.Create();
            }
            using(var stream = file.CreateText())
            {
                stream.Write(data);
            }
        }
        catch(Exception e)
        {
            Console.WriteError($"Error saving report: {e.Message}");
            Console.WriteError(e);
        }

        try
        {
            var data = JsonSerializer.Serialize(results.Results.SelectMany(c=>c.Errors), new JsonSerializerOptions { WriteIndented = true });
            var file = _fileSystem.FileInfo.New(_settings.ErrorPath);
            if (!file.Directory?.Exists ?? false)
            {
                file.Directory.Create();
            }
            using (var stream = file.CreateText())
            {
                stream.Write(data);
            }
        }
        catch(Exception e)
        {
            Console.WriteError($"Error saving errors: {e.Message}");
            Console.WriteError(e);
        }

        return results.Success ? Task.FromResult(ExitCode.Success) : Task.FromResult(ExitCode.Error);
    }

    private SyncResult SyncTarget(SyncTarget target)
    {
        var timer = Stopwatch.StartNew();
        var result = new SyncResult {TargetName = target.Name};
        try
        {
            var sourcePath = (target.Source.EndsWith("\\") ? target.Source.Substring(0, target.Source.Length - 1): target.Source);
            var sourceRoot = _fileSystem.DirectoryInfo.New(sourcePath);
            var destinationPath = (target.Destination.EndsWith("\\") ? target.Destination.Substring(0, target.Destination.Length - 1) : target.Destination);
            var destinationRoot = _fileSystem.DirectoryInfo.New(destinationPath);
            foreach(var sourceFile in sourceRoot.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                var relativePath = sourceFile.FullName.Substring(sourceRoot.FullName.Length + 1);
                var destinationFile = _fileSystem.FileInfo.New(_fileSystem.Path.Combine(destinationRoot.FullName, relativePath));
                if(!destinationFile.Exists || sourceFile.LastWriteTimeUtc > destinationFile.LastWriteTimeUtc || sourceFile.Length != destinationFile.Length)
                {
                    Console.WriteLine($"Copying {sourceFile.FullName}{Environment.NewLine} to {destinationFile.FullName}");
                    try
                    {
                        if(!destinationFile.Directory?.Exists??false)
                        {
                            _fileSystem.Directory.CreateDirectory(destinationFile.Directory.FullName);
                        }
                        _fileSystem.File.Copy(sourceFile.FullName, destinationFile.FullName, true);
                        result.FileCount++;
                    }
                    catch(Exception e)
                    {
                        result.Errors.Add($"Error copying {sourceFile.FullName} to {destinationFile.FullName}: {e.Message}");
                    }
                }
                if(timer.Elapsed.TotalMinutes> _settings.MaxSyncMinutes)
                {
                    Console.WriteWarning($"Max sync time of {_settings.MaxSyncMinutes} minutes exceeded");
                    break;
                }
            }

            timer.Stop();
            result.Duration = timer.Elapsed;
            result.Completed = true;
            return result;
        }
        catch(Exception e)
        {
            Console.WriteError($"Error syncing target {target.Name}: {e.Message}");
            Console.WriteError(e);
            result.Errors.Add($"Critical Error syncing target {target.Name}: {e.Message}"); 
            timer.Stop();
            result.Duration = timer.Elapsed;
            return result;
        }
    }
}
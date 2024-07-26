using System.IO.Abstractions;
using FileSync.Configuration;
using FileSync.Constants;
using FileSync.Options;
using FileSync.Services;

namespace FileSync.Commands;

public class ListTargetsCommand: Command<ListTargetsOptions>
{
    private readonly IFileSystem _fileSystem;
    private readonly Settings _settings;

    public ListTargetsCommand(IConsole console, IFileSystem fileSystem, Settings settings) : base(console)
    {
        _fileSystem = fileSystem;
        _settings = settings;
    }

    protected override Task<int> ExecuteCommandAsync(ListTargetsOptions options)
    {
        foreach(var target in _settings.Targets)
        {
            Console.WriteLine($" Name: {target.Name}");
            Console.WriteLine($" - Source: {target.Source}");
            Console.WriteLine($" - Destination: {target.Destination}");
            Console.WriteLine();
        }

        return Task.FromResult(ExitCode.Success);
    }
}
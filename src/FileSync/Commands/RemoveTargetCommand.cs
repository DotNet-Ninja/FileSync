using FileSync.Configuration;
using FileSync.Constants;
using FileSync.Options;
using FileSync.Services;

namespace FileSync.Commands;

public class RemoveTargetCommand: Command<RemoveTargetOptions>
{
    private readonly Settings _settings;

    public RemoveTargetCommand(IConsole console, Settings settings) : base(console)
    {
        _settings = settings;
    }

    protected override async Task<int> ExecuteCommandAsync(RemoveTargetOptions options)
    {
        var target = _settings.Targets.FirstOrDefault(t => t.Name == options.Name);
        if (target == null)
        {
            Console.WriteError($"Target '{options.Name}' not found.");
            return ExitCode.Error;
        }

        _settings.Targets.Remove(target);
        await _settings.SaveAsync();
        Console.WriteLine($"Target '{options.Name}' removed.");
        return ExitCode.Success;
    }
}
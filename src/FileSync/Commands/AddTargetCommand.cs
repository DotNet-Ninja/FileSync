using FileSync.Configuration;
using FileSync.Constants;
using FileSync.Options;
using FileSync.Services;

namespace FileSync.Commands;

public class AddTargetCommand:Command<AddTargetOptions>
{
    private readonly Settings _settings;

    public AddTargetCommand(IConsole console, Settings settings) : base(console)
    {
        _settings = settings;
    }

    protected override async Task<int> ExecuteCommandAsync(AddTargetOptions options)
    {
        var target = options.ToSyncTarget();
        if(_settings.Targets.Any(t => t.Name == target.Name))
        {
            Console.WriteError($"Target with name '{target.Name}' already exists.");
            return ExitCode.Error;
        }

        _settings.Targets.Add(target);
        await _settings.SaveAsync();
        Console.WriteLine($"Target '{target.Name}' added.");
        return ExitCode.Success;
    }
}
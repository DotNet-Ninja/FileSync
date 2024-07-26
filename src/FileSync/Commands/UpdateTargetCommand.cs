using FileSync.Configuration;
using FileSync.Constants;
using FileSync.Options;
using FileSync.Services;

namespace FileSync.Commands;

public class UpdateTargetCommand: Command<UpdateTargetOptions>
{
    private readonly Settings _settings;

    public UpdateTargetCommand(IConsole console, Settings settings) : base(console)
    {
        _settings = settings;
    }

    protected override async Task<int> ExecuteCommandAsync(UpdateTargetOptions options)
    {
        var target = _settings.Targets.FirstOrDefault(t => t.Name == options.Name);
        if (target == null)
        {
            Console.WriteError($"Target '{options.Name}' not found.");
            return ExitCode.Error;
        }

        options.Apply(target);
        await _settings.SaveAsync();
        
        Console.WriteLine($"Target '{options.NewName}' updated.");

        return ExitCode.Success;
    }
}
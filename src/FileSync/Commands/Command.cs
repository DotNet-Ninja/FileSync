using FileSync.Constants;
using FileSync.Options;
using FileSync.Services;

namespace FileSync.Commands;

public abstract class Command<TOptions> : ICommand<TOptions> where TOptions : class, IOptions, new()
{
    protected Command(IConsole console)
    {
        Console = console;
    }

    protected IConsole Console { get; }

    public async Task<int> ExecuteAsync(TOptions options)
    {
        try
        {
            return await ExecuteCommandAsync(options);
        }   
        catch(Exception e)
        {
            Console.WriteError(e);
            return ExitCode.Error;
        }
    }

    protected abstract Task<int> ExecuteCommandAsync(TOptions options);
}
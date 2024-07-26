using FileSync.Options;

namespace FileSync.Commands;

public interface ICommand<TOptions> where TOptions : class, IOptions, new()
{
    Task<int> ExecuteAsync(TOptions options);
}
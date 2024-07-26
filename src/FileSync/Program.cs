using CommandLine;
using FileSync.Commands;
using FileSync.Configuration;
using FileSync.Constants;
using FileSync.Options;
using Microsoft.Extensions.DependencyInjection;

namespace FileSync;

class Program
{
    private static IServiceProvider _serviceProvider = ServiceProviderBuilder.Build();

    public static async Task<int> Main(string[] args)
    {
        var settings = _serviceProvider.GetRequiredService<Settings>();

        var exitCode = await Parser.Default.ParseArguments<ListTargetsOptions, AddTargetOptions, RemoveTargetOptions, UpdateTargetOptions, RunOptions>(args)
            .MapResult(
                (ListTargetsOptions listTargets) => _serviceProvider.GetRequiredService<ICommand<ListTargetsOptions>>().ExecuteAsync(listTargets),
                (AddTargetOptions addTarget) => _serviceProvider.GetRequiredService<ICommand<AddTargetOptions>>().ExecuteAsync(addTarget),
                (RemoveTargetOptions removeTarget) => _serviceProvider.GetRequiredService<ICommand<RemoveTargetOptions>>().ExecuteAsync(removeTarget),
                (UpdateTargetOptions updateTarget) => _serviceProvider.GetRequiredService<ICommand<UpdateTargetOptions>>().ExecuteAsync(updateTarget),
                (RunOptions run) => _serviceProvider.GetRequiredService<ICommand<RunOptions>>().ExecuteAsync(run),
                errors => Task.FromResult(ExitCode.Error)
            );

        return exitCode;
    }
}

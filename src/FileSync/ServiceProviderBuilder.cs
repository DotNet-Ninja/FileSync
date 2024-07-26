using System.IO.Abstractions;
using FileSync.Commands;
using FileSync.Configuration;
using FileSync.Options;
using FileSync.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileSync;

public static class ServiceProviderBuilder
{
    public static IServiceProvider Build()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(Settings.FilePath, true)
            .Build();
        var fileSystem = new FileSystem();
        var settings = new Settings(fileSystem);
        config.Bind(settings);

        var services = new ServiceCollection();
        services.AddSingleton(settings)
            .AddSingleton<IFileSystem>(fileSystem)
            .AddSingleton<IConsole, SystemConsole>()
            .AddSingleton<ICommand<ListTargetsOptions>, ListTargetsCommand>()
            .AddSingleton<ICommand<AddTargetOptions>, AddTargetCommand>()
            .AddSingleton<ICommand<RemoveTargetOptions>, RemoveTargetCommand>()
            .AddSingleton<ICommand<UpdateTargetOptions>, UpdateTargetCommand>()
            .AddSingleton<ICommand<RunOptions>, RunCommand>();

        return services.BuildServiceProvider();
    }
}
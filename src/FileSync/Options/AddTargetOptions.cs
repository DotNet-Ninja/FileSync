using CommandLine;
using FileSync.Configuration;

namespace FileSync.Options;

[Verb("add-target", HelpText = "Add a new sync target")]
public class AddTargetOptions: IOptions
{
    [Option('n', "name", Required = true, HelpText = "The name of the sync target")]
    public string Name { get; set; } = string.Empty;

    [Option('s', "source", Required = true, HelpText = "The source directory to sync from")]
    public string Source { get; set; } = string.Empty;
    
    [Option('d', "destination", Required = true, HelpText = "The destination directory to sync to")]
    public string Destination { get; set; } = string.Empty;

    public SyncTarget ToSyncTarget()
    {
        return new SyncTarget
        {
            Name = Name,
            Source = Source,
            Destination = Destination
        };
    }
}
using CommandLine;
using FileSync.Configuration;

namespace FileSync.Options;

[Verb("update-target", HelpText = "Update a target.")]
public class UpdateTargetOptions: IOptions
{
    [Option('n', "name", Required = true, HelpText = "The name of the sync target")]
    public string Name { get; set; } = string.Empty;

    [Option('c', "new-name", Required = false, Default = "", HelpText = "The new name of the sync target")]
    public string NewName { get; set; } = string.Empty;


    [Option('s', "source", Required = false, Default = "", HelpText = "The source directory to sync from")]
    public string Source { get; set; } = string.Empty;

    [Option('d', "destination", Required = false, Default = "", HelpText = "The destination directory to sync to")]
    public string Destination { get; set; } = string.Empty;

    public void Apply(SyncTarget target)
    {
        target.Name = NewName == string.Empty ? target.Name : NewName;
        target.Source = Source == string.Empty ? target.Source : Source;
        target.Destination = Destination == string.Empty ? target.Destination : Destination;
    }
}
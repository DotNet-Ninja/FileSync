using CommandLine;

namespace FileSync.Options;

[Verb("remove-target", HelpText = "Remove a target from the configuration.")]
public class RemoveTargetOptions: IOptions
{
    [Option('n', "name", Required = true, HelpText = "The name of the target to remove.")]
    public string Name { get; set; } = string.Empty;
}
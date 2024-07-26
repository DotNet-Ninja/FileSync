using CommandLine;

namespace FileSync.Options;

[Verb("run", HelpText = "Run the file sync process.")]
public class RunOptions: IOptions
{
    [Option('t', "targets", Required = false, HelpText = "The targets to sync.")]
    public IEnumerable<string> Targets { get; set; } = new List<string>();
}
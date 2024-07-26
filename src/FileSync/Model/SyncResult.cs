using System.Text;

namespace FileSync.Model;

public class SyncResult
{
    public string TargetName { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public bool Success => !Errors.Any();
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public int FileCount { get; set; } = 0;
    public bool Completed { get; set; } = false;
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Target: {TargetName}");
        sb.AppendLine($"Success: {Success}");
        sb.AppendLine($"Duration: {Duration}");
        if(Errors.Any())
        {
            sb.AppendLine("Errors:");
            foreach(var error in Errors)
            {
                sb.AppendLine(error);
            }
        }

        return sb.ToString();
    }
}
namespace FileSync.Model;

public class RunResults
{
    public DateTime StartTime { get; set; } = DateTime.Now;
    public List<SyncResult> Results { get; set; } = new();
    public bool Success => Results.All(r => r.Success);
    public int FileCount => Results.Sum(r => r.FileCount);
}
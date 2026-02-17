namespace DealOrNoDeal.Code;

public class DialogueEntry
{
    public string AudioFile { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
}

public class DialogueData
{
    public Dictionary<string, Dictionary<string, DialogueEntry>> Data { get; set; } = new();
}


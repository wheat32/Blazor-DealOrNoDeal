using System.Net.Http.Json;

namespace DealOrNoDeal.Code;

public class DialogueService
{
    private readonly HttpClient _httpClient;
    private Dictionary<string, Dictionary<string, DialogueEntry>>? _dialogueData;

    public DialogueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LoadDialogueAsync()
    {
        if (_dialogueData == null)
        {
            _dialogueData = await _httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, DialogueEntry>>>("data/dialogue.json");
        }
    }

    public DialogueEntry? GetDialogue(string category, string key)
    {
        if (_dialogueData == null)
            return null;

        if (_dialogueData.TryGetValue(category, out var categoryData))
        {
            if (categoryData.TryGetValue(key, out var entry))
            {
                return entry;
            }
        }

        return null;
    }
}


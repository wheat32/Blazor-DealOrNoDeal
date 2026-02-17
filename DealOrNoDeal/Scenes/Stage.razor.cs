using DealOrNoDeal.Code;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DealOrNoDeal.Scenes;

public partial class Stage : ComponentBase, IAsyncDisposable
{
    [Inject]
    private DialogueService DialogueService { get; set; } = null!;
    
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;
    
    private CancellationTokenSource? _cts;
    private readonly HashSet<int> _visibleCases = [];
    
    private bool AllCasesVisible { get; set; }
    private bool CasesClickable { get; set; }
    private String? CurrentSubtitle { get; set; }
    private DotNetObjectReference<Stage>? DotNetHelper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await DialogueService.LoadDialogueAsync();
        DotNetHelper = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Start both audio and animation in parallel
            _ = PlayIntroAudio();
            await AnimateBriefcases();
        }
    }

    private async Task PlayIntroAudio()
    {
        try
        {
            _cts = new CancellationTokenSource();
            
            // Play the deal-cue music
            await PlayAudioFile("audio/music/deal-cue.mp3");
            
            // Wait 8 seconds for the music to set the mood
            await Task.Delay(8000, _cts.Token);
            
            // Play Howie's dialogue with subtitle
            await PlayDialogue("howie", "random-cases-01");
        }
        catch (TaskCanceledException)
        {
            // Task was cancelled, nothing to do
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing audio: {ex.Message}");
        }
    }

    private async Task PlayAudioFile(String audioFile)
    {
        // Create audio element dynamically
        await JSRuntime.InvokeVoidAsync("eval", $@"
            const audio = new Audio('{audioFile}');
            audio.play();
        ");
    }

    private async Task PlayDialogue(String category, String key)
    {
        var dialogue = DialogueService.GetDialogue(category, key);
        if (dialogue == null)
        {
            Console.WriteLine($"Dialogue not found: {category}/{key}");
            return;
        }

        // Show subtitle
        CurrentSubtitle = dialogue.Subtitle;
        StateHasChanged();

        // Create and play audio, wait for it to end
        await JSRuntime.InvokeVoidAsync("eval", $@"
            const audio = new Audio('{dialogue.AudioFile}');
            audio.play();
            audio.addEventListener('ended', () => {{
                window.dialogueEnded = true;
            }});
        ");

        // Poll for audio to finish
        while (true)
        {
            await Task.Delay(100, _cts?.Token ?? CancellationToken.None);
            var ended = await JSRuntime.InvokeAsync<bool>("eval", "window.dialogueEnded || false");
            if (ended)
            {
                await JSRuntime.InvokeVoidAsync("eval", "window.dialogueEnded = false");
                break;
            }
        }

        // Clear subtitle
        CurrentSubtitle = null;
        StateHasChanged();
    }

    private async Task AnimateBriefcases()
    {
        try
        {
            // Wait 1.5 second before starting the animation
            await Task.Delay(1500, _cts?.Token ?? CancellationToken.None);
            
            // Calculate delay between each case appearance
            // 5 seconds total / 26 cases = ~192ms per case
            int delayPerCase = 5000 / 26; // approximately 192ms
            
            // Animate cases 1 through 26 in order
            for (int i = 1; i <= 26; i++)
            {
                _visibleCases.Add(i);
                StateHasChanged();
                
                if (i < 26) // Don't delay after the last case
                {
                    await Task.Delay(delayPerCase, _cts?.Token ?? CancellationToken.None);
                }
            }
            
            AllCasesVisible = true;
            CasesClickable = true;
            StateHasChanged();
        }
        catch (TaskCanceledException)
        {
            // Task was cancelled, nothing to do
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error animating briefcases: {ex.Message}");
        }
    }

    private string GetBriefcaseClass(int caseNumber)
    {
        if (AllCasesVisible)
            return "";
        
        if (_visibleCases.Contains(caseNumber))
            return "appearing";
        
        return "hidden";
    }

    private void OnCaseClicked(int caseNumber)
    {
        if (!CasesClickable)
            return;
            
        // TODO: Handle case click logic
        Console.WriteLine($"Case {caseNumber} clicked");
    }

    public async ValueTask DisposeAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        DotNetHelper?.Dispose();
    }
}


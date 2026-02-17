using Microsoft.AspNetCore.Components;

namespace DealOrNoDeal.Scenes;

public partial class Stage : ComponentBase
{
    private void OnCaseClicked(int caseNumber)
    {
        // TODO: Handle case click logic
        Console.WriteLine($"Case {caseNumber} clicked");
    }
}


using Microsoft.AspNetCore.Components;

namespace DealOrNoDeal.Components;

public partial class Subtitle : ComponentBase
{
    [Parameter]
    public required String Text { get; set; }
    
    [Parameter]
    public bool IsInstruction { get; set; }
}


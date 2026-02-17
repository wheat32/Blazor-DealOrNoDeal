using DealOrNoDeal.Code;
using Microsoft.AspNetCore.Components;

namespace DealOrNoDeal.Pages;

public partial class Home : ComponentBase, IDisposable
{
    protected override void OnInitialized()
    {
        Game.OnStateChanged += StateHasChanged;
    }

    public void Dispose()
    {
        Game.OnStateChanged -= StateHasChanged;
    }
}


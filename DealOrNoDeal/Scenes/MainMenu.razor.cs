using DealOrNoDeal.Code;
using Microsoft.AspNetCore.Components;

namespace DealOrNoDeal.Scenes;

public partial class MainMenu : ComponentBase
{
    private void StartGame()
    {
        Game.StartGame();
        StateHasChanged();
    }
}
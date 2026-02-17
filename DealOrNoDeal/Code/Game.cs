namespace DealOrNoDeal.Code;

public enum GameState
{
    InMainMenu,
    Intro,
    PickingPlayerCase,
    PickingCases,
    ShowingBoard,
    BankersOffer,
    GameOver
}

public static class Game
{
    public static event Action? OnStateChanged;

    public static GameState GameState { get; private set; } = GameState.InMainMenu;
    
    public static List<Case> Cases { get; private set; } = [];
    
    public static void StartGame()
    {
        GameState = GameState.Intro;
        CreateCases();
        OnStateChanged?.Invoke();
    }
    
    private static void CreateCases()
    {
        // Shuffle the case values
        List<double> caseValues = CaseData.CaseValues.OrderBy(_ => Guid.NewGuid()).ToList();
        Cases.Clear();
        for (int i = 0; i < caseValues.Count; i++)
        {
            Cases.Add(new Case(i + 1, caseValues[i]));
        }
    }
}
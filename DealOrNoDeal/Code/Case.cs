namespace DealOrNoDeal.Code;

public class Case
{
    public int Number { get; }
    public double Value { get; }
    public bool IsOpened { get; set; }
    public bool IsPlayerCase { get; set; }
    
    public Case(int number, double value)
    {
        Number = number;
        Value = value;
    }
}
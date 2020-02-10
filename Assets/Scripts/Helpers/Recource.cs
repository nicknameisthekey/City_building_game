[System.Serializable]
public class Recource
{
    public int Amount;
    public RecourceType Type;

    public Recource(RecourceType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}

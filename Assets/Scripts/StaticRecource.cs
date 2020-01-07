[System.Serializable]
public class StaticRecource
{
    public StaticRecourceType Type;
    public int Amount;
    public StaticRecource(StaticRecourceType t, int a)
    {
        Type = t;
        Amount = a;
    }
}

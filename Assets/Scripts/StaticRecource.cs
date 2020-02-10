[System.Serializable]
public class StaticRecource
{
    public GlobalRecourceType Type;
    public int Amount;
    public StaticRecource(GlobalRecourceType t, int a)
    {
        Type = t;
        Amount = a;
    }
}

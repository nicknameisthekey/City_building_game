using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ActiveBuilding", menuName = "new ActiveBuilding Params", order = 51)]
public class ActiveBuildingParams : ScriptableObject
{
    [SerializeField] int ticksToProduce;
    public int TicksToProduce { get => ticksToProduce; }
    [SerializeField] List<Recource> recourcesProduction;
    [SerializeField] List<Recource> constructRecources;
    [SerializeField] List<Recource> inputRecourceCapacity;
    [SerializeField] List<Recource> outputRecourceCapacity;
    public List<Recource> ConstructRecources { get => constructRecources; }
    public Dictionary<RecourceType, int> InputRecources { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecources { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> InputRecourceCapacity { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecourceCapacity { get; private set; } = new Dictionary<RecourceType, int>();
    public bool InputRequired { get; private set; } = false;

    private void OnEnable()
    {
        fillRecourcesDictionaries();
    }
    void fillRecourcesDictionaries()
    {
        foreach (var r in recourcesProduction)
        {
            if (r.Amount >= 0)
                OutputRecources.Add(r.Type, r.Amount);
            else
            {
                InputRequired = true;
                InputRecources.Add(r.Type, r.Amount);
            }
        }
        if (InputRequired)
            foreach (var r in inputRecourceCapacity)
            {
                InputRecourceCapacity.Add(r.Type, r.Amount);
            }
        foreach (var r in outputRecourceCapacity)
        {
            OutputRecourceCapacity.Add(r.Type, r.Amount);
        }
    }
}

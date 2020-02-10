using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ActiveBuilding", menuName = "ActiveBuilding Params", order = 51)]
public class ActiveBuildingParams : BuildingParams
{
    [SerializeField] int ticksToProduce;
    public int TicksToProduce { get => ticksToProduce; }
    [SerializeField] List<Recource> recourcesProduction;
    [SerializeField] List<Recource> inputRecourceCapacity;
    [SerializeField] List<Recource> outputRecourceCapacity;
    [SerializeField] List<StaticRecource> staticRecourceCost;
    public Dictionary<RecourceType, int> InputRecources { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecources { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> InputRecourceCapacity { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecourceCapacity { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<GlobalRecourceType, int> StaticRecourceCost { get; private set; } = new Dictionary<GlobalRecourceType, int>();
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
                InputRecources.Add(r.Type, -r.Amount);
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
        foreach (var r in staticRecourceCost)
        {
            StaticRecourceCost.Add(r.Type, r.Amount);
        }
    }
}

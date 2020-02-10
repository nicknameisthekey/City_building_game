using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New PassiveBuilding", menuName = "PassiveBuilding Params", order = 51)]
public class PassiveBuildingParams : BuildingParams
{
    [SerializeField] List<StaticRecource> staticRecources;
    public Dictionary<GlobalRecourceType, int> StaticRecourcesRequired { get; private set; } = new Dictionary<GlobalRecourceType, int>();
    public Dictionary<GlobalRecourceType, int> StaticRecourcesProvided { get; private set; } = new Dictionary<GlobalRecourceType, int>();
    public bool recourcesRequired { get; private set; } = false;

    private void OnEnable()
    {
        fillRecourcesDictionary();
    }

    void fillRecourcesDictionary()
    {
        foreach (var res in staticRecources)
        {
            if (res.Amount < 0)
            {
                StaticRecourcesRequired.Add(res.Type, -res.Amount);
                recourcesRequired = true;
            }
            else
                StaticRecourcesProvided.Add(res.Type, res.Amount);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New PassiveBuilding", menuName = "PassiveBuilding Params", order = 51)]
public class PassiveBuildingParams : BuildingParams
{
    [SerializeField] List<StaticRecource> staticRecources;
    public Dictionary<StaticRecourceType, int> StaticRecourcesRequired { get; private set; } = new Dictionary<StaticRecourceType, int>();
    public Dictionary<StaticRecourceType, int> StaticRecourcesProvided { get; private set; } = new Dictionary<StaticRecourceType, int>();
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
                StaticRecourcesRequired.Add(res.Type, res.Amount);
                recourcesRequired = true;
            }
            else
                StaticRecourcesProvided.Add(res.Type, res.Amount);
        }
    }

}

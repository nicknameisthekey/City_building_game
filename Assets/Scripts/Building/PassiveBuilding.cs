using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBuilding : BuildingNearRoad
{
    [SerializeField] List<StaticRecourceType> StaticRecourcesTypes;
    [SerializeField] List<int> Amounts;
    Dictionary<StaticRecourceType, int> _staticRecourcesRequired = new Dictionary<StaticRecourceType, int>();
    Dictionary<StaticRecourceType, int> _staticRecourcesProvided = new Dictionary<StaticRecourceType, int>();
    bool recourcesRequired = false;
    public override void Initialize(Vector2Int tileID, Vector2Int roadIDItConnects)
    {
        base.Initialize(tileID, roadIDItConnects);
        fillRecourcesDictionary();
        changeStaticRecources();
    }
    void fillRecourcesDictionary()
    {
        for (int i = 0; i < StaticRecourcesTypes.Count; i++)
        {
            if (Amounts[i] < 0)
            {
                _staticRecourcesRequired.Add(StaticRecourcesTypes[i], Amounts[i]);
                recourcesRequired = true;
            }
            else
                _staticRecourcesProvided.Add(StaticRecourcesTypes[i], Amounts[i]);
        }
    }

    void changeStaticRecources()
    {
        StaticRecources.RecourcesChanged -= changeStaticRecources;
        if (recourcesRequired)
        {
            if (StaticRecources.ChangeAmount(_staticRecourcesRequired))
                StaticRecources.ChangeAmount(_staticRecourcesProvided);
            else
                StaticRecources.RecourcesChanged += changeStaticRecources;
        }
        else
            StaticRecources.ChangeAmount(_staticRecourcesProvided);
    }

}

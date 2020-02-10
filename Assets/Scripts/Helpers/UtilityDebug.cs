using UnityEngine;

public class UtilityDebug : MonoBehaviour
{
    public static bool ActivebuildingLog { get; private set; }
    public static bool PassivebuildingLog { get; private set; }
    public static bool BuildingConstructionLog { get; private set; }
    public void Initialization(GameSettings settings)
    {
        ActivebuildingLog = settings.ActiveBuildingLog;
        PassivebuildingLog = settings.PassiveBuildingLog;
        BuildingConstructionLog = settings.BuildingConstructionLog;
    }
}

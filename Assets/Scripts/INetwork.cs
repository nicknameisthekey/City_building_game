using UnityEngine;

public interface INetwork
{
    RoadNetwork CurrentNetwork { get; }
    void ChangeRoadNetwork(RoadNetwork newNetwork);
}

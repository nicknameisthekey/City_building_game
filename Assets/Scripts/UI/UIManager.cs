using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    [SerializeField] GameObject buildingUIPrefab;
    [SerializeField] GameObject recourceInfoPrefab;
    [SerializeField] GameObject staticRecourcePannelPrefab;
    [SerializeField] GameObject activebuildingUIPrefab;
    Transform UITransform;
    static GameObject _buildingUI;
    static GameObject _recourceInfoPrefab;
    static GameObject _staticRecourcesPanel;
    static GameObject _activeBuildingUI;


    private void Awake()
    {
        instance = this;
        UITransform = gameObject.transform;
        instantiateUI();
    }

    void instantiateUI()
    {
        _buildingUI = Instantiate(buildingUIPrefab, UITransform);
        _recourceInfoPrefab = Instantiate(recourceInfoPrefab, UITransform);
        _recourceInfoPrefab.SetActive(false);
        _staticRecourcesPanel = Instantiate(staticRecourcePannelPrefab, UITransform);
        _activeBuildingUI = Instantiate(activebuildingUIPrefab, UITransform);
    }


}

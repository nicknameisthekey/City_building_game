using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviour
{
    [SerializeField] GameObject recourceImagePrefab;
    [SerializeField] Transform layout;
    [SerializeField] Sprite rawFood;
    [SerializeField] Sprite food;
    [SerializeField] Sprite wood;
    static Dictionary<RecourceType, RecourceImage> recourceImages = new Dictionary<RecourceType, RecourceImage>();
    static NetworkInfo instance;
    static RoadNetwork currentNetwork;
    private void Awake()
    {
        instance = this;
        foreach (var item in Enum.GetValues(typeof(RecourceType)))
        {
            var Image = Instantiate(recourceImagePrefab, layout);
            RecourceImage recImage = Image.GetComponent<RecourceImage>();
            recImage.ChangeRecourceText("0");
            recImage.ChangeRecorceImage(getRightSprite((RecourceType)item));
            recourceImages.Add((RecourceType)item, recImage);
        }
    }

    public static void ShowRecourcesOnNetwork(RoadNetwork network)
    {
        instance.gameObject.SetActive(true);
        UpdateRecourceInfo(network.Recources);
        currentNetwork = network;
        network.RecourcesChanged += UpdateRecourceInfo;
    }
    public static void UpdateRecourceInfo(Dictionary<RecourceType, float> recources)
    {

        foreach (var item in recourceImages)
        {
            item.Value.ChangeRecourceText("0");
        }
        foreach (var item in recources)
        {
            recourceImages[item.Key].ChangeRecourceText(item.Value.ToString());
        }
    }
    public static void HideRecourceInfo()
    {
        instance.gameObject.SetActive(false);
        currentNetwork.RecourcesChanged -= UpdateRecourceInfo;

    }
    Sprite getRightSprite(RecourceType recource)
    {
        switch (recource)
        {
            case RecourceType.food: return food;
            case RecourceType.rawFood: return rawFood;
            case RecourceType.wood: return wood;
            default: return null;
        }
    }

}

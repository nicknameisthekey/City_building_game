using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageInfo : MonoBehaviour
{
    [SerializeField] GameObject recourceImagePrefab;
    [SerializeField] Transform layout;
    [SerializeField] Sprite rawFood;
    [SerializeField] Sprite food;
    [SerializeField] Sprite wood;
    static Dictionary<RecourceType, RecourceImage> recourceImages = new Dictionary<RecourceType, RecourceImage>();
    static StorageInfo instance;
    static Storage currentStorage;
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

    public static void ShowRecources(Storage storage)
    {
        instance.gameObject.SetActive(true);
        UpdateRecourceInfo(storage.Recources);
        currentStorage = storage;
        storage.RecourceChanged += UpdateRecourceInfo;
    }
    public static void UpdateRecourceInfo(RecourceType type, float amount)
    {
        recourceImages[type].ChangeRecourceText(amount.ToString());
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
        currentStorage.RecourceChanged -= UpdateRecourceInfo;

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

﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageInfo : MonoBehaviour
{
    [SerializeField] GameObject recourceImagePrefab;
    [SerializeField] Transform layout;
    [SerializeField] Text totalStorageAmountText;
    [SerializeField] ImagesData recourceIcons;
    static Dictionary<RecourceType, RecourceImage> recourceImages = new Dictionary<RecourceType, RecourceImage>();
    static StorageInfo instance;
    static Storage currentStorage;
    private void Awake()
    {
        instance = this;
        InputHandler.CloseAllWindowsPressed += HideRecourceInfo;
        foreach (var item in Enum.GetValues(typeof(RecourceType)))
        {
            RecourceType rectype = (RecourceType)item;
            if (rectype == RecourceType.all)
                continue;
            var Image = Instantiate(recourceImagePrefab, layout);
            RecourceImage recImage = Image.GetComponent<RecourceImage>();
            recImage.ChangeRecourceText("0");
            recImage.ChangeRecorceImage(recourceIcons.Sprites[(int)rectype]);
            recourceImages.Add(rectype, recImage);
        }
    }

    public static void ShowRecources(Storage storage)
    {
        instance.gameObject.SetActive(true);
        UpdateRecourceInfo(storage);
        currentStorage = storage;
        storage.RecourceChanged += UpdateRecourceInfo;
    }
    public static void UpdateRecourceInfo(RecourceType type, int amount, Storage storage)
    {
        recourceImages[type].ChangeRecourceText(amount.ToString());
        instance.totalStorageAmountText.text = "Заполненность склада: " + storage.TotalAmountOfGoods + "/" + storage.Capacity;
    }
    public static void UpdateRecourceInfo(Storage storage)
    {
        foreach (var item in recourceImages)
        {
            item.Value.ChangeRecourceText("0");
        }
        foreach (var item in storage.Recources)
        {
            recourceImages[item.Key].ChangeRecourceText(item.Value.ToString());
        }
        instance.totalStorageAmountText.text = "Заполненность склада: " + storage.TotalAmountOfGoods + "/" + storage.Capacity;
    }
    public static void HideRecourceInfo()
    {
        if (!instance.gameObject.activeSelf) return;
        instance.gameObject.SetActive(false);
        currentStorage.RecourceChanged -= UpdateRecourceInfo;
    }
}

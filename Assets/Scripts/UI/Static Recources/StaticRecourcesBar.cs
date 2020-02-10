using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticRecourcesBar : MonoBehaviour
{
    [SerializeField] GameObject StaticRecourceImagePrefab;
    [SerializeField] GameObject Layout;
    [SerializeField] ImagesData recourceSprites;  
    Dictionary<GlobalRecourceType, Text> texts = new Dictionary<GlobalRecourceType, Text>();

    private void Awake()
    {
        foreach (var item in Enum.GetValues(typeof(GlobalRecourceType)))
        {
            GlobalRecourceType type = (GlobalRecourceType)item;
            GameObject image = Instantiate(StaticRecourceImagePrefab, Layout.transform);
            Text text = image.GetComponentInChildren<Text>();
            image.GetComponent<Image>().sprite = recourceSprites.Sprites[(int)type];
            texts.Add(type, text);
        }
        foreach (var kvp in texts)
        {
            kvp.Value.text = GlobalRecources.GetAmount(kvp.Key).ToString();
        }

        GlobalRecources.RecourceChanged += changeAmount;
    }
    void changeAmount(GlobalRecourceType type)
    {
       // Debug.Log(StaticRecources.GetAmount(type).ToString());
        texts[type].text = GlobalRecources.GetAmount(type).ToString();
    }

}

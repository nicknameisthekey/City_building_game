using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticRecourcesBar : MonoBehaviour
{
    [SerializeField] GameObject StaticRecourceImagePrefab;
    [SerializeField] GameObject Layout;
    [SerializeField] List<Sprite> recourceSprites;  //переделать в SO?
    Dictionary<StaticRecourceType, Text> texts = new Dictionary<StaticRecourceType, Text>();

    private void Awake()
    {
        //foreach (var item in Enum.GetValues(typeof(StaticRecourceType)))
        //{
        //    StaticRecourceType type = (StaticRecourceType)item;
        //    GameObject image = Instantiate(StaticRecourceImagePrefab, Layout.transform);
        //    Text text = image.GetComponentInChildren<Text>();
        //    image.GetComponent<Image>().sprite = recourceSprites[(int)type];
        //    texts.Add(type, text);
        //}
        //if (MapGenerator.MapCreated)
        //{
        //    foreach (var kvp in texts)
        //    {
        //        kvp.Value.text = StaticRecources.GetAmount(kvp.Key).ToString();
        //    }
        //}
        //    StaticRecources.RecourceChanged += changeAmount;
    }
    void changeAmount(StaticRecourceType type)
    {
        Debug.Log(StaticRecources.GetAmount(type).ToString());
        texts[type].text = StaticRecources.GetAmount(type).ToString();
    }

}

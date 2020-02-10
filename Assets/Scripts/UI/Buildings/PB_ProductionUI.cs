using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PB_ProductionUI : MonoBehaviour
{
    [SerializeField] GameObject imagePrefab;
    [SerializeField] ImagesData staticRecourceIcons;
    [SerializeField] Transform outputLayout;
    [SerializeField] Transform inputLayout;
    GameObject[] outputImages = new GameObject[4];
    GameObject[] inputImages = new GameObject[4];
    PB_ProductionCycle currentState;
    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, outputLayout);
            outputImages[i] = image;
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, inputLayout);
            inputImages[i] = image;
        }
        gameObject.SetActive(false);
    }
    public void Show(PB_ProductionCycle state)
    {
        if (currentState == null)
        {
            currentState = state;
            gameObject.SetActive(true);
            currentState.StaticRecourcesChanged += updateInfo;
            GlobalRecources.RecourcesChanged += updateInfo;
            updateInfo();
        }
        else
        {
            Close();
            Show(state);
        }
    }
    void updateInfo()
    {
        KeyValuePair<GlobalRecourceType, int>[] inputKVPS;
        KeyValuePair<GlobalRecourceType, int>[] outputKVPS = currentState.PBParams.StaticRecourcesProvided.ToArray(); ;
        int i = 0;
        if (currentState.PBParams.recourcesRequired)
        {
            inputKVPS = currentState.PBParams.StaticRecourcesRequired.ToArray();
            for (i = 0; i < inputKVPS.Length; i++)
            {
                inputImages[i].SetActive(true);
                inputImages[i].GetComponent<Image>().sprite = staticRecourceIcons.Sprites[(int)inputKVPS[i].Key];
                inputImages[i].GetComponentInChildren<Text>().text = $"{GlobalRecources.GetAmount(inputKVPS[i].Key)} / " +
                    $"{inputKVPS[i].Value}";
            }
        }
        for (; i < 4; i++)
        {
            inputImages[i].SetActive(false);
        }
        for (i = 0; i < outputKVPS.Length; i++)
        {
            outputImages[i].SetActive(true);
            outputImages[i].GetComponent<Image>().sprite = staticRecourceIcons.Sprites[(int)outputKVPS[i].Key];
            outputImages[i].GetComponentInChildren<Text>().text = $"{outputKVPS[i].Value}";
        }
        for (; i < 4; i++)
        {
            outputImages[i].SetActive(false);
        }
    }
    public void Close()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
        currentState.StaticRecourcesChanged -= updateInfo;
        GlobalRecources.RecourcesChanged -= updateInfo;
        currentState = null;
    }
}

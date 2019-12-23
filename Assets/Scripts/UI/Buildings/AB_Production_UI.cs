using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AB_Production_UI : MonoBehaviour
{
    [SerializeField] GameObject imagePrefab;
    [SerializeField] ImagesData recourceIcons;
    [SerializeField] Transform outputLayout;
    [SerializeField] Transform outputStorageLayout;
    [SerializeField] Image progressBar;
    GameObject[] outputImages = new GameObject[4];
    GameObject[] outputStorageImages = new GameObject[4];
    AB_State_ProductionCycle currentState;
    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, outputLayout);
            outputImages[i] = image;
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, outputStorageLayout);
            outputStorageImages[i] = image;
        }
        gameObject.SetActive(false);
    }
    public void Show(AB_State_ProductionCycle state)
    {
        if (currentState == null)
        {
            currentState = state;
            gameObject.SetActive(true);
            state.OutputAdded += updateInfo;
            state.OutputSubstracted += updateInfo;
            state.ab_Work.ProgressChanged += updateInfo;
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
        int i = 0;
        var outputCapKVPS = currentState.building.AbParams.OutputRecourceCapacity.ToList();
        var outputKVPS = currentState.building.AbParams.OutputRecources.ToList();
        for (; i < outputKVPS.Count; i++)
        {
            outputImages[i].SetActive(true);
            outputImages[i].GetComponent<Image>().sprite = recourceIcons.Sprites[(int)outputKVPS[i].Key];
            outputImages[i].GetComponentInChildren<Text>().text = outputKVPS[i].Value.ToString();

        }
        for (; i < 4; i++)
        {
            outputImages[i].SetActive(false);
        }
        i = 0;
        for (; i < outputCapKVPS.Count; i++)
        {
            outputStorageImages[i].SetActive(true);
            outputStorageImages[i].GetComponent<Image>().sprite = recourceIcons.Sprites[(int)outputCapKVPS[i].Key];
            outputStorageImages[i].GetComponentInChildren<Text>().text =
            currentState.OutputRecourcesLocal[outputCapKVPS[i].Key].ToString() + "/" +
            outputCapKVPS[i].Value;
        }
        for (; i < 4; i++)
        {
            outputStorageImages[i].SetActive(false);
        }
        progressBar.GetComponent<Image>().fillAmount = (float)currentState.ab_Work.ticksDone / (float)currentState.ab_Work.ticksNeed;
    }
    public void Close()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
        currentState.OutputAdded -= updateInfo;
        currentState.OutputSubstracted -= updateInfo;
        currentState.ab_Work.ProgressChanged -= updateInfo;
        currentState = null;
    }
}

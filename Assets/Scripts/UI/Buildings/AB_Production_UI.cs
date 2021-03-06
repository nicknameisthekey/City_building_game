﻿using System.Collections.Generic;
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
    [SerializeField] StartStopBTN startStopBTN;
    [SerializeField] Transform inputStorageLayout;
    [SerializeField] Transform staticRecourcesLayout;
    [SerializeField] ImagesData staticRecourcesIcons;
    GameObject[] inputStorageImages = new GameObject[4];
    GameObject[] outputImages = new GameObject[4];
    GameObject[] outputStorageImages = new GameObject[4];
    GameObject[] staticRecourcesImages = new GameObject[4];
    AB_State_ProductionCycle currentState;
    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, inputStorageLayout);
            inputStorageImages[i] = image;
        }
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
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, staticRecourcesLayout);
            staticRecourcesImages[i] = image;
        }
        gameObject.SetActive(false);
    }
    public void Show(AB_State_ProductionCycle state)
    {
        if (currentState == null)
        {
            currentState = state;
            startStopBTN.Initialize(state.Building);
            gameObject.SetActive(true);
            state.OutputAdded += updateInfo;
            state.OutputSubstracted += updateInfo;
            state.Ab_Work.ProgressChanged += updateInfo;
            state.InputSubstracted += updateInfo;
            state.InputAdded += updateInfo;
            state.StaticRecourcesProvided += updateInfo;
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
        if (currentState.Building.AbParams.InputRequired)
        {
            var inputCapKVPS = currentState.Building.AbParams.InputRecourceCapacity.ToList();
            var inputKVPS = currentState.Building.AbParams.InputRecources.ToList();

            for (; i < inputCapKVPS.Count; i++)
            {
                inputStorageImages[i].SetActive(true);
                inputStorageImages[i].GetComponent<Image>().sprite = recourceIcons.Sprites[(int)inputCapKVPS[i].Key];
                inputStorageImages[i].GetComponentInChildren<Text>().text =
                currentState.InputRecourcesLocal[inputCapKVPS[i].Key].ToString() + "/" +
                inputCapKVPS[i].Value;
            }
            for (; i < 4; i++)
            {
                inputStorageImages[i].SetActive(false);
            }
        }
        else
        {
            for (; i < 4; i++)
            {
                inputStorageImages[i].SetActive(false);
            }
        }

        i = 0;
        var outputCapKVPS = currentState.Building.AbParams.OutputRecourceCapacity.ToList();
        var outputKVPS = currentState.Building.AbParams.OutputRecources.ToList();
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
        //staticres
        KeyValuePair<GlobalRecourceType, int>[] staticKVP = currentState.Building.AbParams.StaticRecourceCost.ToArray();
        i = 0;
        for (; i < staticKVP.Length; i++)
        {
            staticRecourcesImages[i].SetActive(true);
            staticRecourcesImages[i].GetComponent<Image>().sprite = staticRecourcesIcons.Sprites[(int)staticKVP[i].Key];
            staticRecourcesImages[i].GetComponentInChildren<Text>().text = staticKVP[i].Value.ToString();
            if (currentState.StaticRecourceProvided_bool)
                staticRecourcesImages[i].GetComponentInChildren<Text>().color = Color.green;
            else
                staticRecourcesImages[i].GetComponentInChildren<Text>().color = Color.red;
        }
        for (; i < 4; i++)
        {
            staticRecourcesImages[i].SetActive(false);
        }
        progressBar.GetComponent<Image>().fillAmount = (float)currentState.Ab_Work.ticksDone / (float)currentState.Ab_Work.ticksNeed;
    }
    public void Close()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
        currentState.OutputAdded -= updateInfo;
        currentState.OutputSubstracted -= updateInfo;
        currentState.Ab_Work.ProgressChanged -= updateInfo;
        currentState.InputSubstracted -= updateInfo;
        currentState.InputAdded -= updateInfo;
        currentState.StaticRecourcesProvided -= updateInfo;
        currentState = null;
    }
}

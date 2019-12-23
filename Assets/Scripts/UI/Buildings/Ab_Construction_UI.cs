using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ab_Construction_UI : MonoBehaviour
{
    [SerializeField] GameObject imagePrefab;
    [SerializeField] ImagesData recourceIcons;
    [SerializeField] Transform Layout;
    GameObject[] images = new GameObject[4];
    AB_State_CollectingMaterials currentState;
    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, Layout);
            images[i] = image;
        }
        gameObject.SetActive(false);
    }
    public void Show(AB_State_CollectingMaterials state)
    {
        if (currentState == null)
        {
            currentState = state;
            gameObject.SetActive(true);
            updateInfo(state);
            state.RecourcesLeftChanged += updateInfo;
            state.StateChanged += onStateChanged;
        }
        else
        {
            Close();
            Show(state);
        }
    }
    void onStateChanged(ActiveBuildingState newstate)
    {
        var st = currentState;
        Close();
        ActiveBuildingUI.ShowUI(st.building);
    }
    void updateInfo(AB_State_CollectingMaterials state)
    {
        int i = 0;
        for (; i < state.building.AbParams.ConstructRecources.Count; i++)
        {
            if (state.recourcesLeftToDeliver.ContainsKey(state.building.AbParams.ConstructRecources[i].Type))
            {
                images[i].SetActive(true);
                images[i].GetComponent<Image>().sprite = recourceIcons.Sprites[(int)state.building.AbParams.ConstructRecources[i].Type];
                images[i].GetComponentInChildren<Text>().text = state.recourcesLeftToDeliver[state.building.AbParams.ConstructRecources[i].Type].ToString();
            }
        }
        for (; i < 4; i++)
        {
            images[i].SetActive(false);
        }
    }
    public void Close()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
        currentState.StateChanged -= onStateChanged;
        currentState.RecourcesLeftChanged -= updateInfo;
        currentState = null;
    }
}

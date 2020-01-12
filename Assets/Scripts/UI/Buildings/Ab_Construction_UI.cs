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
    State_Construction currentState;
    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject image = Instantiate(imagePrefab, Layout);
            images[i] = image;
        }
        gameObject.SetActive(false);
    }
    public void Show(State_Construction state)
    {
        if (currentState == null)
        {
            currentState = state;
            gameObject.SetActive(true);
            updateInfo(state);
            state.RecourcesLeftChanged += updateInfo;
            state.Building.StateChanged += onStateChanged;
        }
        else
        {
            Close();
            Show(state);
        }
    }
    void onStateChanged(BuildingState newstate)
    {
        var st = currentState;
        Close();
        ActiveBuildingUI.ShowUI((ActiveBuilding)st.Building);
    }
    void updateInfo(State_Construction state)
    {
        int i = 0;
        ActiveBuilding ab =(ActiveBuilding) state.Building;
        for (; i < ab.AbParams.ConstructRecources.Count; i++)
        {
            if (state.RecourcesLeftToDeliver.ContainsKey(ab.AbParams.ConstructRecources[i].Type))
            {
                images[i].SetActive(true);
                images[i].GetComponent<Image>().sprite = recourceIcons.Sprites[(int)ab.AbParams.ConstructRecources[i].Type];
                images[i].GetComponentInChildren<Text>().text = state.RecourcesLeftToDeliver[ab.AbParams.ConstructRecources[i].Type].ToString();
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
        currentState.Building.StateChanged -= onStateChanged;
        currentState.RecourcesLeftChanged -= updateInfo;
        currentState = null;
    }
}

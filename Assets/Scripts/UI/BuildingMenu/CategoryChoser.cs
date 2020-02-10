using System;
using UnityEngine;

public class CategoryChoser : MonoBehaviour
{
    public static event Action CategoryChoosed = delegate { };
    [SerializeField] GameObject categoryPannel;
    void Awake()
    {
        CategoryChoosed += hidePannel;
    }

   public void OnClick()
    {
        CategoryChoosed.Invoke();
        categoryPannel.SetActive(true);
    }
    void hidePannel()
    {
        categoryPannel.SetActive(false);
    }
}

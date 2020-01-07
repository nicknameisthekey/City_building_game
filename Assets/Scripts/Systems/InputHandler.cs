using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    KeyCode rotationKey;
    [SerializeField]
    KeyCode showInfo;
    [SerializeField]
    KeyCode closeAllWindows;
    [SerializeField]
    KeyCode closeAllWindowsAlt;
    public static event Action RotationKeyPressed = delegate { };
    public static event Action ShowInfoPressed = delegate { };
    public static event Action CloseAllWindowsPressed = delegate { };
    private void Update()
    {
        if (Input.GetKeyDown(rotationKey))
            RotationKeyPressed.Invoke();
        if (Input.GetKeyDown(showInfo) && !EventSystem.current.IsPointerOverGameObject())
        {
            CloseAllWindowsPressed.Invoke();
            ShowInfoPressed.Invoke();
        }
        if (Input.GetKeyDown(closeAllWindows) || Input.GetKeyDown(closeAllWindowsAlt))
            CloseAllWindowsPressed.Invoke();
    }
}

using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    KeyCode rotationKey;
    public static event Action RotationKeyPressed = delegate { };
    private void Update()
    {
        if (Input.GetKeyDown(rotationKey))
            RotationKeyPressed.Invoke();
    }
}

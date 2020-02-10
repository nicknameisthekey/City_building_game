using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static event Action OneSecond = delegate { };
    private void Awake()
    {
        StartCoroutine(oneSec());
    }

    IEnumerator oneSec()
    {
        while (true)
        {
            OneSecond.Invoke();
            yield return new WaitForSeconds(1f);
        }
    }
}

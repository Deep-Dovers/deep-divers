using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public static bool IsOpen { get; private set; } = false;

    private void Awake()
    {
        
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
    }

    public virtual void ToggleWindow()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }
}

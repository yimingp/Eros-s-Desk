using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
}

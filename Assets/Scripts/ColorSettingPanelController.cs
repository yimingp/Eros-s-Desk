using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ColorSettingPanelController : MonoBehaviour
{
    [Title("Data")]
    public List<ColorSettingController> colorSettings;

    public void Start()
    {
        for (var i = 0; i < TitleManager.Instance.palette.Count; ++i)
        {
            colorSettings[i].Initialize(TitleManager.Instance.palette[i]);
        }
    }
}

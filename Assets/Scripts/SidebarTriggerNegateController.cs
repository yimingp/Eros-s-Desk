using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SidebarTriggerNegateController : MonoBehaviour, IPointerEnterHandler
{
    public SidebarController sidebar;

    public bool isUsing;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        SetUsing(false);
    }

    public void SetUsing(bool status)
    {
        isUsing = status;
        _image.raycastTarget = status;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUsing)
        {
            sidebar.OnPointerExit();
            SetUsing(false);
        }
    }
}

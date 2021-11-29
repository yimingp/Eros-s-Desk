using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SidebarTriggerController : MonoBehaviour, IPointerEnterHandler
{
    public SidebarController sidebarController;
    public SidebarTriggerNegateController negatetor;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        sidebarController.OnPointerEnter();
        negatetor.SetUsing(true);
    }
}

using System;
using Unit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Boundary
{
    public class Background : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (InputRequestManager.Instance.canRequest && Input.GetMouseButtonUp(0))
                OnUnitSelectController.Instance.OnUnitDeselect();
        }
    }
}
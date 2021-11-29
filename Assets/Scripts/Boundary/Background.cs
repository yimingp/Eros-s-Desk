using System;
using Unit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Boundary
{
    public class Background : MonoBehaviour, IPointerClickHandler
    {
        public UnitRelationshipGUIDrawer relationshipGUIDrawer;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(0))
            {
                relationshipGUIDrawer.UndoDrawing();
            }
        }
    }
}
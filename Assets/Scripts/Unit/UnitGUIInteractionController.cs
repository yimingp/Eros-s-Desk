using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unit
{
    public class UnitGUIInteractionController : MonoBehaviour, IPointerClickHandler
    {
        [Title("Reference")] 
        public Unit unit;

        public UnitRelationshipGUIDrawer relationshipGUIDrawer;

        private void Awake()
        {
            relationshipGUIDrawer = FindObjectsOfType<UnitRelationshipGUIDrawer>().First(e => e.isMainDrawer);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(0))
                OnLeftClick();
            else if (Input.GetMouseButtonUp(1))
                OnRightClick();
        }

        private void OnLeftClick()
        {
            ShowRelations();
        }

        private void OnRightClick()
        {
        }

        private void ShowRelations()
        {
            
            relationshipGUIDrawer.DrawUnitRelations(unit);
        }
    }
}
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unit
{
    public class UnitGUIInteractionController : MonoBehaviour, IPointerClickHandler
    {
        [Title("Reference")] 
        public Unit unit;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(0))
                OnLeftClick();
            else if (Input.GetMouseButtonUp(1))
                OnRightClick();
        }

        private void OnLeftClick()
        {
            Debug.Log("onleftclick");
            ShowRelations();
        }

        private void OnRightClick()
        {
            Debug.Log("onrightclick");
        }

        private void ShowRelations()
        {
            
            UnitRelationshipGUIDrawer.Instance.DrawUnitRelations(unit);
        }
    }
}
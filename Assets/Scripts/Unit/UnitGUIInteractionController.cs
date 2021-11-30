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

        private OnUnitSelectController _selectController;

        private void Start()
        {
            _selectController = OnUnitSelectController.Instance;
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
            if(_selectController != null)
                _selectController.OnUnitSelect(unit);
        }

        private void OnRightClick()
        {
        }
    }
}
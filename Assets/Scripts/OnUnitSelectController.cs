using System;
using Sirenix.OdinInspector;
using Unit;
using UnityEngine;

public class OnUnitSelectController : MonoBehaviour
{
    public static OnUnitSelectController Instance;

    [Title("Reference")]
    public SelectorController selector;
    public UnitRelationshipGUIDrawer relationshipGUIDrawer;
    public ProfileCardController profileCardController;
    
    
    private void Awake()
    {
        Instance = this;
    }

    public void OnUnitSelect(Unit.Unit unit)
    {
        selector.SetFollow(unit.transform);
        selector.gameObject.SetActive(true);
        relationshipGUIDrawer.DrawUnitRelations(unit);
        /*profileCardController.SetProfilingUnit(unit);
        profileCardController.OnEnter();*/
    }

    public void OnUnitDeselect(Unit.Unit unit = null)
    {
        selector.gameObject.SetActive(false);
        relationshipGUIDrawer.UndoDrawing();
        /*
        profileCardController.OnExit();
    */
    }
}
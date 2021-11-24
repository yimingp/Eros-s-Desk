using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class MousePusher : MonoBehaviour
{
    [Title("Reference")]
    public AreaEffector2D effector;
    
    private Vector2 _lastMousePos;
    
    public void UpdatePushAngle()
    {
        var direction = (GetMousePosition() - _lastMousePos).normalized;
        effector.forceAngle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
    }
    
    private void Update()
    {
        UpdatePushAngle();
        transform.position = GetMousePosition();
    }

    private void LateUpdate()
    {
        _lastMousePos = GetMousePosition();
    }

    private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
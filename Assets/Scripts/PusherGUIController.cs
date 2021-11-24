using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PusherGUIController : MonoBehaviour
{

    [Title("Setting")]
    public bool canUse = true;
    
    [Title("Reference")]
    public MousePusher pusher;

    private void Update()
    {
        if(!canUse)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            pusher.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            pusher.gameObject.SetActive(false);
        }
    }
}

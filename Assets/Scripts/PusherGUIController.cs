using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PusherGUIController : MonoBehaviour
{

    [Title("Setting")]
    public bool canUse = false;
    
    [Title("Reference")]
    public MousePusher pusher;

    public void ToggleUsing()
    {
        canUse = !canUse;

        if (!canUse)
        {
            pusher.gameObject.SetActive(false);
        }
    }
    
    
    private void Update()
    {
        if(!canUse)
            return;

        if (InputRequestManager.Instance.canRequest)
        {
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
}

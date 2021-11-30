using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class InputRequestManager : MonoBehaviour
{
    public static InputRequestManager Instance;

    [Title("Data")] 
    public bool canRequest = true;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCanRequest(bool val)
    {
        canRequest = val;
    }

    public void ToggleCanRequest()
    {
        canRequest = !canRequest;
    }
}
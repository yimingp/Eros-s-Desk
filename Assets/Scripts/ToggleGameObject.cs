using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGameObject : MonoBehaviour
{
    public GameObject go;

    public void Toggle()
    {
        go.SetActive(!go.activeSelf);
    }
}

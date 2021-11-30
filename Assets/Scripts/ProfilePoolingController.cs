using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProfilePoolingController : MonoBehaviour
{
    [Title("Reference")] 
    
    public GameObject nameTextPrefab;
    public GameObject logTextPrefab;

    public Transform nameTextPool;
    public Transform logTextPool;

    private List<GameObject> _nameTextFreePool;
    private List<GameObject> _nameTextUsingPool;
    private List<GameObject> _logTextFreePool;
    private List<GameObject> _logTextUsingPool;

    private void Start()
    {
        _nameTextFreePool = new List<GameObject>();
        _nameTextUsingPool = new List<GameObject>();
        _logTextFreePool = new List<GameObject>();
        _logTextUsingPool = new List<GameObject>();
    }

    public GameObject GetNameText()
    {
        GameObject go;

        if (_nameTextFreePool.Count > 0)
        {
            go = _nameTextFreePool[0];
            _nameTextFreePool.Remove(go);
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(nameTextPrefab, nameTextPool);
        }
        
        _nameTextUsingPool.Add(go);

        return go;
    }

    public void ReturnNameText(GameObject nameText)
    {
        var success = _nameTextUsingPool.Remove(nameText);
        if (!success) return;
        _nameTextFreePool.Add(nameText);
        nameText.SetActive(false);
    }

    public GameObject GetLogText()
    {
        GameObject go;

        if (_logTextFreePool.Count > 0)
        {
            go = _logTextFreePool[0];
            _logTextFreePool.Remove(go);
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(logTextPrefab, logTextPool);
        }
        
        _logTextUsingPool.Add(go);
        
        return go;
    }

    public void ReturnLogText(GameObject logText)
    {
        var success = _logTextUsingPool.Remove(logText);
        if (!success) return;
        _logTextFreePool.Add(logText);
        logText.SetActive(false);
    }
}

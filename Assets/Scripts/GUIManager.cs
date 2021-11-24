using System;
using System.Collections.Generic;
using Relation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;

    public SerializedDictionary<RelationType, Color> drawColors;

    private void Awake()
    {
        Instance = this;
    }
}

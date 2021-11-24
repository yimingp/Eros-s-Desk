using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public static TitleManager Instance;

    [Title("Setting")] public int numSpawns = 100;
    //public List<Color> palette;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToPlayScene()
    {
        SceneManager.LoadScene("Main");
    }
}

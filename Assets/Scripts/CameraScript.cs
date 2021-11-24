using System;
using Cinemachine;
using Sirenix.OdinInspector;
using Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    
    [Title("Setting")]
    public float followTime;
    
    [Title("Reference")] 
    public CinemachineVirtualCamera cam;
    public UnitsManager manager;

    private float _timeCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _timeCounter = followTime;
    }

    private void LateUpdate()
    {
        if(cam.Follow == null)
            RandomFollow();
        
        _timeCounter -= Time.deltaTime;

        if (_timeCounter <= 0)
        {
            RandomFollow();
        }
    }

    [Button]
    public void RandomFollow()
    {
        _timeCounter = followTime;
        cam.Follow = manager.units.TakeRandomElementFromList().gameObject.transform;
    }
}
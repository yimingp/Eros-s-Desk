using System.Collections.Generic;
using System.Linq;
using Affinities;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using Unit;

public class GameManager : MonoBehaviour
{
    [Title("Setting")] 
    public bool autoSpawn;

    [Title("Data")]
    public GameObject player;
    public List<GameObject> units;
    
    [Title("Reference")]
    public GameInitializer gameInitializer;
    public UnitsManager unitManager;
    public ColorAffinitiesController affinityController;

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        affinityController.InitializeAffinities();

        if (autoSpawn)
        {
            units = gameInitializer.StartInitialize();
            unitManager.units = units.Select(unit => unit.GetComponent<Unit.Unit>()).ToList();
            player = units[Random.Range(0, units.Count - 1)];
        }
    }

    private void GameFinish()
    {
        Debug.Log("Game Finished!");
    }
}

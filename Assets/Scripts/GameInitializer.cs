using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameInitializer : MonoBehaviour
{
    [Title("Reference")]
    public GameObject unit;
    public Transform units;
    public GameObject field;
    public UnitIDGenerator idGenerator;
    public UnitsManager unitsManager;
    
    [Title("Color")]
    public UnitColorController colorController;
    public float colorDistributionMargin;
    private float[] _colorDistribution;
    private List<KeyValuePair<int,int>> _eachColorCounts;

    [Title("Spawn Setting")] 
    public bool useCustomStep;
    [ShowIf("useCustomStep")] public int step;
    public int spawns;
    private int _spawnCount;

    private float _fieldRatio;
    private float _spreadModelDiff;
    private float _spreadSizeMax;
    private Vector2 _spreadSize;
    private Vector2 _spawnOrigin;
    
    private Vector2 _fieldSize;
    private Vector2 _fieldCenter;
    private HashSet<Vector2> _toSpawnList;
    private List<Vector2> _spreadModel;

    private void Start()
    {
        spawns = TitleManager.Instance.numSpawns;
    }


    public List<GameObject> StartInitialize()
    {
        var position = field.transform.position;
        var scale = field.transform.localScale;

        _fieldCenter = position;
        _fieldSize = scale;
        _fieldSize.x -= 1;
        _fieldSize.y -= 1;

        
        _spawnOrigin = new Vector2(position.x - scale.x / 2,
            position.y - scale.y / 2);

        _toSpawnList = new HashSet<Vector2>();
        _spreadModel = new List<Vector2>();

        _spawnCount = 0;
        _fieldRatio = _fieldSize.x / _fieldSize.y;

        if (useCustomStep)
        {
            _spreadSizeMax = step * spawns;
        
            // calculate spread model
            _spreadSize.y = Mathf.Sqrt(_spreadSizeMax / _fieldRatio);
            _spreadSize.x = Mathf.FloorToInt(_spreadSize.y * _fieldRatio);
            _spreadSize.y = (int) _spreadSize.y;

            _spreadModelDiff = scale.x / _spreadSize.x;
        }
        else
        {
            
        }
        
        
        var spreadModelChecker = new bool[(int)_spreadSize.x, (int)_spreadSize.y];

        var yPtr = 0;
        var xPtr = 0;

        Random.InitState((int)System.DateTime.Now.Ticks);
        
        do
        {
            spreadModelChecker[xPtr, yPtr] = true;
            _spreadModel.Add(new Vector2(xPtr, yPtr));
            ++_spawnCount;

            var nextStep = Random.Range(1, step);
            if (xPtr + nextStep >= _spreadSize.x)
            {
                ++yPtr;
                xPtr = (int)((xPtr + nextStep) - _spreadSize.x);
            }
            else
            {
                xPtr += nextStep;
            }
            
        } while (yPtr < _spreadSize.y && _spawnCount < spawns);
        
        // convert spread model to real grid position
        _spreadModel.ForEach(pos =>
        {
            var loc = pos * _spreadModelDiff;
            loc.x = (int) loc.x;
            loc.y = (int) loc.y;
            _toSpawnList.Add(loc);
        });
        
        // instantiate from grid position with vibration

        var unitList = new List<GameObject>();
        
        UnitColorDistributionInitialize();
        
        foreach (var pos in _toSpawnList)
        {
            // calculate vibration
            var spawnPos = new Vector2(pos.x, pos.y * _fieldRatio) + _spawnOrigin;
            spawnPos.x += Random.Range(-1f, 1f)+2;
            spawnPos.y += Random.Range(-1f, 1f)+2;
            
            if(spawnPos.x >= _fieldSize.x / 2 + _fieldCenter.x 
               || spawnPos.x <= _fieldCenter.x - _fieldSize.x / 2
               || spawnPos.y >=  _fieldSize.y / 2 + _fieldCenter.y 
               || spawnPos.y <= _fieldCenter.y - _fieldSize.y / 2 ) 
                continue;
            
            var newUnit = Instantiate(unit, spawnPos, Quaternion.identity, units);
            newUnit = UnitAutoCustomization(newUnit);
            unitList.Add(newUnit);
        }

        return unitList;
    }

    private void UnitColorDistributionInitialize()
    {
        _colorDistribution = new float[colorController.palette.Count];
        _eachColorCounts = new List<KeyValuePair<int, int>>(_colorDistribution.Length);
        var eachColorDist = 1 / (float)_colorDistribution.Length;
        var distMargin = eachColorDist * colorDistributionMargin;
        var distCounter = 0f;
        for (var i = 0; i < _colorDistribution.Length; ++i)
        {
            var dist = eachColorDist + Random.Range(-distMargin, distMargin);
            
            if (distCounter + dist > 1)
            {
                dist = 1 - distCounter;
            }
            else
            {
                distCounter += dist;    
            }
            
            _colorDistribution[i] = dist;
        }

        for (var i = 0; i < _eachColorCounts.Capacity; i++)
        {
            _eachColorCounts.Add(new KeyValuePair<int, int>(i, Mathf.FloorToInt(_colorDistribution[i] * _toSpawnList.Count)));
        }
    }

    private GameObject UnitAutoCustomization(GameObject newUnit)
    {
        var unitScript = newUnit.GetComponent<Unit.Unit>();
        unitScript.Initialize(unitsManager);
        
        var randomPick = Random.Range(0, _eachColorCounts.Count);
        var palette = colorController.palette;
        Color pickedColor;
        
        if (_eachColorCounts.Count > 0)
        {
            var pickedIndex = _eachColorCounts[randomPick].Key;
            pickedColor = palette[pickedIndex];
        }
        else
        {
            pickedColor = palette[0];
        }
        
        unitScript.SetColor(pickedColor);
        
        unitScript.id = idGenerator.GetNextId();

        if (_eachColorCounts.Count > 0)
        {
            _eachColorCounts[randomPick] =
                new KeyValuePair<int, int>(_eachColorCounts[randomPick].Key, _eachColorCounts[randomPick].Value - 1);
            if (_eachColorCounts[randomPick].Value <= 0)
            {
                _eachColorCounts.RemoveAt(randomPick);
            }
        }


        return newUnit;
    }
}
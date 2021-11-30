using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Title("Setting")]
    public float moveInTime;
    public float moveOutTime;
    public float fadeInTime;
    public float fadeOutTime;

    [Title("Reference")] 
    public ProfilePoolingController poolingController;
    public Transform endPosition;

    [Title("SettingText")] 
    public InputField nameText;
    public Text ageText;
    public GameObject family;
    public GameObject friend;
    public GameObject enemy;
    public GameObject logs;

    private List<GameObject> families;
    private List<GameObject> friends;
    private List<GameObject> enemies;

    private Vector2 _startPosition;

    private Unit.Unit _profilingUnit;
    private Image _image;
    private float _imageAlphaCached;

    private void Start()
    {
        _image = GetComponent<Image>();
        _imageAlphaCached = _image.color.a;
        _image.color = _image.color.ModifiedAlpha(0);
        _startPosition = transform.position;

        families = new List<GameObject>();
        friends = new List<GameObject>();
        enemies = new List<GameObject>();
    }

    public void SetProfilingUnit(Unit.Unit unit)
    {
        _profilingUnit = unit;

        unit.OnNewRelation += UpdateUnitRelationChart;
        
        nameText.text = unit.unitName;
        UpdateUnitRelationChart();
    }

    private void Update()
    {
        ageText.text = "" + (int)_profilingUnit.lifeTime;
    }

    private void UpdateUnitRelationChart()
    {
        ClearRelationCharts();
        _profilingUnit.Relations.ForEach(r =>
        {
            
        });
    }

    private void ClearRelationCharts()
    {
        
    }

    public void OnEnter()
    {
        transform.DOMoveX(endPosition.transform.position.x, moveInTime);
        _image.DOFade(_imageAlphaCached, fadeInTime);
    }

    public void OnExit()
    {
        transform.DOMoveX(_startPosition.x, moveOutTime);
        _image.DOFade(0, fadeOutTime);

        _profilingUnit.OnNewRelation -= UpdateUnitRelationChart;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InputRequestManager.Instance.SetCanRequest(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputRequestManager.Instance.SetCanRequest(true);
    }
}

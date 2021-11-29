using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SidebarController : MonoBehaviour
{
    [Title("Setting")]
    public float fadeInTime;
    public float fadeOutTime;
    public float moveInTime;
    public float moveOutTime;
    
    
    [Title("Reference")]
    public Transform endPosition;
    private Vector2 _startPosition;

    private float _endAlphaValue;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _endAlphaValue = _image.color.a;
        _image.color = _image.color.ModifiedAlpha(0);
        _startPosition = transform.position;
    }

    public void OnPointerEnter()
    {
        transform.DOMoveX(endPosition.position.x, moveInTime);
        _image.DOFade(_endAlphaValue, fadeInTime);
    }

    public void OnPointerExit()
    {
        transform.DOMoveX(_startPosition.x, moveOutTime);
        _image.DOFade(0, fadeOutTime);
    }
}

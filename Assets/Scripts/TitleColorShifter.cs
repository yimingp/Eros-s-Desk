using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TitleColorShifter : MonoBehaviour
{
    [Title("Settting")] 
    public float colorChangeSpeed;
    public float colorLightMinimum;
    
    [Title("Reference")]
    public Image titleText;

    private Color _textColor;
    private ColorExtension.ColorChannelType _currentChannel = ColorExtension.ColorChannelType.R;
    private bool _colorLightGoingDown = true;
    
    private void Start()
    {
        _textColor = titleText.color;
    }

    private void Update()
    {
        ShiftingNeonColor();
        titleText.color = _textColor;
    }

    private void ShiftingNeonColor()
    {
        switch (_currentChannel)
        {
            case ColorExtension.ColorChannelType.R:
                _textColor = _colorLightGoingDown ? 
                    _textColor.ModifiedRChannel(_textColor.r - colorChangeSpeed) 
                    : _textColor.ModifiedRChannel(_textColor.r + colorChangeSpeed);
                if (_textColor.r <= colorLightMinimum)
                    _colorLightGoingDown = !_colorLightGoingDown;
                else if (_textColor.r >= 1)
                {
                    _colorLightGoingDown = true;
                    _currentChannel = ColorExtension.ColorChannelType.G;
                }
                break;
            case ColorExtension.ColorChannelType.G:
                _textColor = _colorLightGoingDown ? 
                    _textColor.ModifiedGChannel(_textColor.g - colorChangeSpeed) 
                    : _textColor.ModifiedGChannel(_textColor.g + colorChangeSpeed);
                if (_textColor.g <= colorLightMinimum)
                    _colorLightGoingDown = !_colorLightGoingDown;
                else if (_textColor.g >= 1)
                {
                    _colorLightGoingDown = true;
                    _currentChannel = ColorExtension.ColorChannelType.B;
                }
                break;
            case ColorExtension.ColorChannelType.B:
                _textColor = _colorLightGoingDown ? 
                    _textColor.ModifiedBChannel(_textColor.b - colorChangeSpeed) 
                    : _textColor.ModifiedBChannel(_textColor.b + colorChangeSpeed);
                if (_textColor.b <= colorLightMinimum)
                    _colorLightGoingDown = !_colorLightGoingDown;
                else if (_textColor.b >= 1)
                {
                    _colorLightGoingDown = true;
                    _currentChannel = ColorExtension.ColorChannelType.R;
                }
                break;
            default:
                break;
        }   
    }
}

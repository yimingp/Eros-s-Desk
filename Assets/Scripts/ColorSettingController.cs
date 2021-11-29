using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSettingController : MonoBehaviour
{
    public Image colorImage;
    public InputField colorInput;

    public void Initialize(Color color)
    {
        ChangeColor(color);
    }

    private void ChangeColor(Color color)
    {
        colorImage.color = color;
        colorInput.text = "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    public void OnInputChange(string input)
    {
        Color newColor = default;
        var success = ColorUtility.TryParseHtmlString(input,out newColor);
        if (success)
        {
            ChangeColor(newColor);
        }
    }
}

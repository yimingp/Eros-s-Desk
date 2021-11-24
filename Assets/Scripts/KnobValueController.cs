using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnobValueController : MonoBehaviour
{
    public Text text;

    public void OnChangeValue(float val)
    {
        text.text = "" + decimal.Round((decimal)val, 1);
    }
}

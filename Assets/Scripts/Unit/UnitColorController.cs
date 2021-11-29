using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unit
{
    public class UnitColorController : MonoBehaviour
    {
        [Title("Data")]
        public List<Color> palette;

        private void Awake()
        {
            if (TitleManager.Instance != null)
            {
                palette = TitleManager.Instance.palette;
            }
        }
    }
}
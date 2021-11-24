using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Affinities
{
    [System.Serializable]
    public class Affinity
    {
        [ShowInInspector] public Dictionary<Color, AffinityData> Data;

        public Affinity()
        {
            Data = new Dictionary<Color, AffinityData>();
        }
    }
}
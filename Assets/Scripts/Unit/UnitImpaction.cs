using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitImpaction : MonoBehaviour
    {
        [HideInInspector] public int followIndex;
        public bool isReadyToAdvance;
        public bool isCounterClockwise;
        public int length;
        public int capacity;
        public Color color;
        
        public LineRenderer line;
        [HideInInspector] public List<Vector3> points;

        private void Start()
        {
            line.positionCount = length;
            line.startColor = color;
            line.endColor = color;
            points ??= new List<Vector3>();
            for (var i = 0; i < length; ++i)
            {
                points.Add(Vector3.zero);
            }
        }

        public void AddImpact(int magnitude)
        {
            if (points.Count == capacity)
            {
                isReadyToAdvance = true;
                return;
            }
            if (points.Count + magnitude > capacity)
            {
                magnitude = capacity - points.Count;
            }
            
            for (var i = 0; i < magnitude; ++i)
            {
                points.Add(Vector3.zero);
            }

            length = points.Count;
            line.positionCount = points.Count;
        }

        public void ResetToPositions()
        {
            line.SetPositions(points.ToArray());
        }
    }
}
 
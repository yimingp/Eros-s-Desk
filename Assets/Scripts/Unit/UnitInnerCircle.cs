using System;
using System.Collections.Generic;
using System.Linq;
using Affinities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit
{
    public class UnitInnerCircle : MonoBehaviour
    {
        [Title("Setting")] 
        public int vertexCount;
        public float radius;
        public float circulateSpeed;
        public float impactionSpeed;
        [Space] 
        public int impactTolerance;
        
        [Title("Reference")] 
        public GameObject impactionPrefab;
        
        private float _circulationCounter;

        [Title("Data")] 
        [HideInInspector]public Color myColor;
        public List<UnitImpaction> impacts;
        public List<UnitImpaction> fulledImpacts;
        private Vector3[] _points;
        
        private void Awake()
        {
            _points = new Vector3[vertexCount];
            impacts ??= new List<UnitImpaction>();
            fulledImpacts ??= new List<UnitImpaction>();
            _circulationCounter = circulateSpeed;
            InitPoints();
        }

        private void InitPoints()
        {
            var deltaTheta = (Mathf.PI * 2) / vertexCount;
            var theta = 0f;

            for (var i = 0; i < vertexCount; i++)
            {
                var pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                var newPos = transform.localPosition + pos;
                _points[i] = newPos;
                theta += deltaTheta;
            }
        }

        public KeyValuePair<Color, int>[] GetAllImpactColors()
        {
            return (impacts.Concat(fulledImpacts)).Distinct().Select(e => new KeyValuePair<Color, int>(e.color, e.length)).ToArray();
        }

        public float GetAllImpactAffinities()
        {
            var allColors = GetAllImpactColors();

            if (allColors.Length <= 0)
                return 0;

            var sum = 0f;
            float temp;
            
            allColors.ForEach(c =>
            {
                temp = ColorAffinitiesController.Instance.GetAffinity(myColor, c.Key).generalAffinity;
                temp *= (temp > 0.5f) ? (1 + c.Value * 0.05f) : (1 - c.Value * 0.05f);
                sum += temp;
            });

            sum /= allColors.Length;
            
            return sum;
        }

        private void MaintainImpactPosition(UnitImpaction impact)
        {
            for (var i = 0; i < impact.points.Count; ++i)
            {
                var lastIndex = impact.isCounterClockwise ? impact.followIndex - i : impact.followIndex + i;
                if (lastIndex < 0 && impact.isCounterClockwise)
                {
                    lastIndex = _points.Length - (i - impact.followIndex);
                }
                else if (lastIndex >= _points.Length && !impact.isCounterClockwise)
                {
                    lastIndex -= _points.Length;
                }
                impact.points[i] = Vector3.Lerp(impact.points[i], transform.TransformPoint(_points[lastIndex]), impactionSpeed * Time.deltaTime);
            }
            impact.ResetToPositions();
        }

        private void UpdateImpactPosition(UnitImpaction impact)
        {
            var newIndex = impact.isCounterClockwise ? GetNextIndex(impact.followIndex) : GetLastIndex(impact.followIndex);
            for (var i = 0; i < impact.points.Count; ++i)
            {
                var lastIndex = impact.isCounterClockwise ? newIndex - i : newIndex + i;
                if (lastIndex < 0 && impact.isCounterClockwise)
                {
                    lastIndex = _points.Length - (i - newIndex);
                }
                else if (lastIndex >= _points.Length && !impact.isCounterClockwise)
                {
                    lastIndex -= _points.Length;
                }
                impact.points[i] = Vector3.Lerp(impact.points[i], transform.TransformPoint(_points[lastIndex]), impactionSpeed * Time.deltaTime);
            }
            impact.followIndex = newIndex;
            impact.ResetToPositions();
        }

        public void AddToImpact(Color color, int magnitude)
        {
            if (impacts.Count >= impactTolerance) 
                return;
            
            var impact = impacts.Find(impact => impact.color == color);
            if (impact is null)
            {
                // create new impact
                impact = Instantiate(impactionPrefab, transform.position, Quaternion.identity, transform).GetComponent<UnitImpaction>();
                impact.color = color;
                impact.length = magnitude;
                impact.followIndex = Random.Range(0, _points.Length);
                impact.isCounterClockwise = Random.Range(0f, 1f) >= .5f ? true : false;
                impacts.Add(impact);
            }
            else
            {
                impact.AddImpact(magnitude);
                if (impact.isReadyToAdvance && !fulledImpacts.Contains(impact))
                {
                    fulledImpacts.Add(impact);
                }
            }
            MaintainImpactPosition(impact);
            impact.UpdateRenderIndex();
        }

        private void Update()
        {
            _circulationCounter -= Time.deltaTime;
            
            if (impacts.Count <= 0)
                return;

            if (_circulationCounter > 0)
            {
                foreach (var impact in impacts)
                {
                    MaintainImpactPosition(impact);
                }
            }
            else
            {
                _circulationCounter = circulateSpeed;
            
                foreach (var impact in impacts)
                {
                    UpdateImpactPosition(impact);
                }
            }
        }

        private int GetNextIndex(int index)
        {
            var newIndex = index + 1;
            return newIndex >= _points.Length ? 0 : newIndex;
        }

        private int GetLastIndex(int index)
        {
            var newIndex = index - 1;
            return newIndex < 0 ? _points.Length - 1 : newIndex;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var deltaTheta = (Mathf.PI * 2) / vertexCount;
            var theta = 0f;

            var oldPos = Vector3.zero;
            for (var i = 0; i < vertexCount+1; i++)
            {
                var pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                var newPos = transform.localPosition + pos;
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(oldPos, newPos);
                oldPos = newPos;
                theta += deltaTheta;
            }
        }
#endif
    }
}

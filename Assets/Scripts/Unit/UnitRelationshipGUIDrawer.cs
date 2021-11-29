using System;
using System.Collections.Generic;
using Relation;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Unit
{
    public class UnitRelationshipGUIDrawer : MonoBehaviour
    {
        [Title("Setting")] 
        public bool isMainDrawer = false;
        public bool canDraw = true;
        
        [Title("Reference")]
        public GameObject linePrefab;
        
        private bool _isDrawing;
        private Unit _drawingFor;

        private List<Transform> _drawingLineTo;
        private List<LineRenderer> _drawingPool;
        private List<GameObject> _unusedPool;
        private List<GameObject> _usingPool;

        private void Awake()
        {
            _isDrawing = false;
            _drawingPool = new List<LineRenderer>();
            _drawingLineTo = new List<Transform>();
            _unusedPool = new List<GameObject>();
            _usingPool = new List<GameObject>();
        }

        private void LateUpdate()
        {
            if (_isDrawing)
            {
                for (var i = 0; i < _drawingLineTo.Count; i++)
                {
                    _drawingPool[i].SetPosition(0, _drawingFor.transform.position);
                    _drawingPool[i].SetPosition(1, _drawingLineTo[i].position);
                }
            }
        }

        public void UndoDrawing()
        {
            _drawingPool.ForEach(ReturnToPool);
            _drawingPool.Clear();
            _drawingLineTo.Clear();
        }

        public void DrawUnitRelations(Unit unit)
        {
            if (!canDraw)
                return;
            
            _isDrawing = true;
            _drawingFor = unit;
            
            UndoDrawing();

            _drawingFor.Relations.ForEach(relation =>
            {
                var line = GetLineFromPool();
                line.startColor = line.endColor = GUIManager.Instance.drawColors[relation.Value.type];
                line.positionCount = 2;
                line.SetPosition(0, _drawingFor.transform.position);
                line.SetPosition(1, relation.Key.transform.position);
                _drawingLineTo.Add(relation.Key.transform);
                _drawingPool.Add(line);
            });
        }

        private LineRenderer GetLineFromPool()
        {
            GameObject line;
            
            if (_unusedPool.Count > 0)
            {
                line = _unusedPool[0];
                _unusedPool.Remove(line);
                _usingPool.Add(line);
                line.SetActive(true);
            }
            else
            {
                line = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
                _usingPool.Add(line);
            }

            return line.GetComponent<LineRenderer>();
        }
        
        private void ReturnToPool(LineRenderer line)
        {
            ReturnToPool(line.gameObject);
        }

        private void ReturnToPool(GameObject line)
        {
            _usingPool.Remove(line);
            _unusedPool.Add(line);
            line.SetActive(false);
        }
    }
}
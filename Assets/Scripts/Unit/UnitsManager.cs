using System.Collections.Generic;
using System.Linq;
using Affinities;
using Relation;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Relation = Relation.Relation;

namespace Unit
{
    public class UnitsManager : MonoBehaviour
    {
        [TitleGroup("Settings")]
        
        [Title("Settings/LifeTime")]
        public float unitLifeTimeMin;
        public float unitLifeTimeMax;
        
        [Title("Settings/Movement")]
        public float speed;
        public float orbitSpeed;
        public float orbitThetaChangeSpeed;
        public float orbitDesignedRadius;
        public float roamingShiftBtwTime;
        public float bounceOffMagnitude;
        public float bounceEdgeMagnitude;
        public float velocityMax;

        [Title("Settings/Graphic")] 
        public float newSpawnFadingTime;
        [Space]public List<float> unitShadeAlphaLevels;



        [Title("Reference")] 
        public GameObject unitPrefab;
        public Transform unitHierarchy;
        public UnitInteractionManager interactionManager;
        public UnitIDGenerator idGenerator;
        public ColorAffinitiesController colorAffinityController;
//        public ProceduralAudioManager proceduralAudioManager;
        
        [Title("Data")]
        public List<Unit> units;
        private List<UnitInteraction> _interactionPool;
        private List<UnitInteraction> _interactionOccupied;

        private void Awake()
        {
            units ??= new List<Unit>();
            _interactionPool ??= new List<UnitInteraction>();
            _interactionOccupied ??= new List<UnitInteraction>();
        }

        public void AddNewUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void UnitDies(Unit unit)
        {
            units.Remove(unit);
        }

        public UnitInteraction GetInteractionFromPool()
        {
            if (_interactionPool.Count > 0)
            {
                var interaction = _interactionPool[0];
                _interactionPool.Remove(interaction);
                _interactionOccupied.Add(interaction);
                return interaction;
            }
            else
            {
                var newInteraction = new UnitInteraction(interactionManager);
                _interactionOccupied.Add(newInteraction);
                return newInteraction;
            }
        }

        public void ReturnInteractionToPool(UnitInteraction interaction)
        {
            var exist = _interactionOccupied.Remove(interaction);
            if (exist)
            {
                _interactionPool.Add(interaction);
            }            
        }

        public void SpawnNewUnitWithInheritence(Unit inheritFrom, bool setAsFamily = false)
        {
            var spawnPos = inheritFrom.transform.position;
            spawnPos += (Vector3.up * Random.Range(-5f, 5f) + Vector3.right * Random.Range(-5f, 5f));
            var unitObj = Instantiate(unitPrefab, spawnPos, quaternion.identity, unitHierarchy);
            var unitScript = unitObj.GetComponent<Unit>();
            
            unitScript.Initialize(this);
            unitScript.FadeIn();
            unitScript.id = idGenerator.GetNextId();

            var baseColor = inheritFrom.color;
            var impactedColors = inheritFrom.outer.GetAllImpactColors().Concat(inheritFrom.inner.GetAllImpactColors());

            var colorKPs = impactedColors as KeyValuePair<Color, int>[] ?? impactedColors.ToArray();
            var selected = Random.Range(0, 100);
            Color selectedColor = default;
            var summation = 0;
            
            foreach (var colorKP in colorKPs.OrderBy(e => e.Value))
            {
                summation += colorKP.Value;

                if (selected < summation)
                {
                    selectedColor = colorKP.Key;
                    break;
                }
            }

            if (selectedColor == default)
            {
                selectedColor = baseColor;
            }
            
            unitScript.SetColor(selectedColor);

            if (setAsFamily)
            {
                unitScript.AddRelation(inheritFrom, RelationType.Family);
                inheritFrom.AddRelation(unitScript, RelationType.Family);
            }
        }

        public void AddImpactSound()
        {
            
        }

        public void ChangeMaxSpeed(float val)
        {
            velocityMax = val;
        }
    }
}
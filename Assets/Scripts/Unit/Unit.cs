using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Relation;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Relation = Relation.Relation;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        public Action OnNewRelation;
        
        
        [Title("Data")] 
        public uint id;
        public string unitName;
        public float lifeTime;
        public Color color;
        public List<Unit> interactingList;
        [DictionaryDrawerSettings(KeyLabel = "Unit", ValueLabel = "Relation"),ShowInInspector] 
        public Dictionary<Unit, global::Relation.Relation> Relations;
        [DictionaryDrawerSettings(KeyLabel = "RelationType", ValueLabel = "Count"),ShowInInspector] 
        public Dictionary<RelationType, uint> NumberOfEachRelation;
        private List<UnitInteraction> _interactions;

        [Title("Reference")] 
        public UnitMovement movement;
        public UnitInnerCircle outer;
        public UnitInnerCircle inner;
        public SpriteRenderer shade;
        public UnitsManager manager;

        private bool _isFadingIn;
        private float _lifeTimeMax;
        private SpriteRenderer _mySprite;
        private TrailRenderer _myTrail;

        public void Initialize(UnitsManager unitsManager)
        {
            _mySprite = GetComponent<SpriteRenderer>();
            _myTrail = GetComponent<TrailRenderer>();
            movement = GetComponent<UnitMovement>();
            interactingList ??= new List<Unit>();
            _interactions ??= new List<UnitInteraction>();
            Relations = new Dictionary<Unit, global::Relation.Relation>();
            NumberOfEachRelation = new Dictionary<RelationType, uint>()
            {
                {RelationType.Neutral, 0},
                {RelationType.Friend, 0},
                {RelationType.Family, 0},
                {RelationType.Enemy, 0},
            };
            movement.manager = unitsManager;
            manager ??= unitsManager;
            manager.AddNewUnit(this);
            lifeTime = Random.Range(manager.unitLifeTimeMin, manager.unitLifeTimeMax);
            _lifeTimeMax = lifeTime;
        }

        public void FadeIn()
        {
            _isFadingIn = true;
            shade.color = Color.black;
            
            shade.DOFade(0, manager.newSpawnFadingTime).OnComplete(() => _isFadingIn = false);
        }

        public void SetColor(Color newColor)
        {
            color = newColor;
            _mySprite.color = newColor;
            _myTrail.startColor = new Color(newColor.r, newColor.g, newColor.b, 120);
            _myTrail.endColor = new Color(newColor.r, newColor.g, newColor.b, 0);
            inner.myColor = color;
            outer.myColor = color;
        }

        public void NewInteraction(Unit unit)
        {
            interactingList.Add(unit);
            var interaction = manager.GetInteractionFromPool();
            interaction.Clear();
            interaction.SetBothSides(this, unit);
            if(color != unit.color)
                interaction.SetAffinities(manager.colorAffinityController.GetAffinity(color, unit.color).generalAffinity);
            _interactions.Add(interaction);
        }

        public void ExitInteraction(Unit unit)
        {
            var existed = interactingList.Remove(unit);
            if (!existed)
                return;
            var interaction = _interactions.Find(item => item.me == this);
            if (interaction is null)
                return;
            else
            {
                _interactions.Remove(interaction);
                manager.ReturnInteractionToPool(interaction);
            }
        }

        public float GetInfluenceAffinity()
        {
            var innerAff = this.inner.GetAllImpactAffinities();

            innerAff *= (innerAff > 0.5f) ? 1.25f : 0.75f;

            var sum = innerAff + outer.GetAllImpactAffinities();

            sum *= 0.5f;
            
            return sum;
        }

        public void AddRelation(Unit you, RelationType relation)
        {
            if (Relations.ContainsKey(you))
            {
                var oldRel = Relations[you].type;
                if (oldRel == relation)
                    return;
                NumberOfEachRelation[oldRel] -= 1;
                Relations[you].type = relation;
                NumberOfEachRelation[relation] += 1;
            }
            else
            {
                Relations.Add(you, new global::Relation.Relation(this, you, relation));
                NumberOfEachRelation[relation]++;
            }
            
            OnNewRelation?.Invoke();
        }

        public void SetOrbitAround(Unit unit, float howLong, RelationType type = RelationType.Friend)
        {
            if (unit is null)
                return;
            
            if (!movement.isOrbiting || type == RelationType.Family)
            {
                if (unit.movement.orbitingAround == transform)
                {
                    movement.orbitDirection = unit.movement.orbitDirection;
                }
                else
                {
                    movement.orbitDirection = (Random.value < 0.5f) ? -1 : 1;
                }
                
                
                
                movement.SetOrbitingAround(unit, howLong);
            }
        }

        public void AddImpact(Color impactColor, int magnitude)
        {
            outer.AddToImpact(impactColor, magnitude);

            if (inner.impacts.Count >= inner.impactTolerance)
                return;
            
            foreach (var impact in outer.fulledImpacts)
            {
                inner.AddToImpact(impact.color, 2);
                outer.impacts.Remove(impact);
                Destroy(impact.gameObject);
            }
            
            outer.fulledImpacts.Clear();
        }

        private void Update()
        {
            lifeTime -= Time.deltaTime;
            
            UpdateAlphas();

            if (lifeTime <= 0)
            {
                Dies();
                return;
            }

            foreach (var interaction in _interactions)
            {
                interaction.PassTime(Time.deltaTime);
            }
        }

        private void UpdateAlphas()
        {
            if (_isFadingIn)
                return;

            var newAlpha = (_lifeTimeMax - lifeTime) / _lifeTimeMax;

            for (var i = 1; i < manager.unitShadeAlphaLevels.Count; ++i)
            {
                if (newAlpha > manager.unitShadeAlphaLevels[i])
                {
                    newAlpha = manager.unitShadeAlphaLevels[i - 1];
                    break;
                }
            }

            newAlpha = manager.unitShadeAlphaLevels[manager.unitShadeAlphaLevels.Count - 1];
            
            shade.color = new Color(Color.black.r, Color.black.g, Color.black.b, newAlpha);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherUnit = other.transform.GetComponent<Unit>();
            if (otherUnit is null)
                return;
            else
            {
                movement.ApplyOutsideForce((transform.position - other.transform.position).normalized, manager.bounceOffMagnitude, false, true);
                
                if (color == otherUnit.color) return;
                var affinity = manager.colorAffinityController.Affinities[color].Data[otherUnit.color];
                if (Random.value < affinity.collisionImpactRate)
                {
                    AddImpact(otherUnit.color, affinity.collisionImpactMagnitude);
                }
            }
        }

        private void RemoveRelationWith(Unit unit)
        {
            if (!Relations.ContainsKey(unit)) return;
            NumberOfEachRelation[Relations[unit].type]--;
            Relations.Remove(unit);
        }

        private void Dies()
        {
            var interactingUnits = interactingList.Select(e => e).ToArray();
            
            foreach (var unit in interactingUnits)
            {
                ExitInteraction(unit);
            }

            foreach (var relation in Relations)
            {
                relation.Key.RemoveRelationWith(this);
            }
            
            manager.SpawnNewUnitWithInheritence(this);
            
            manager.UnitDies(this);
            
            Destroy(gameObject);
        }

        [Button]
        public void SpawnOffspring()
        {
            manager.SpawnNewUnitWithInheritence(this, true);
        }
    }
}
using System;
using Affinities;
using Relation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit
{
    [System.Serializable]
    public class UnitInteraction
    {
        public float totalInteractionTime;
        public Unit me;
        public Unit you;
        public Relation.Relation myRelationToYou;

        private float _relationshipAmplifyFactor;
        private float _interactionCheckTimer = 0;

        private float _myAffinityToYou;
        private float _impactAccumulationThreshold;
        private float _impactAccumulateCounter;

        private static UnitInteractionManager _manager;
        
        public UnitInteraction(UnitInteractionManager manager)
        {
            _manager ??= manager;
            Clear();
        }
        
        public UnitInteraction(Unit me, Unit you)
        {
            totalInteractionTime = 0;
            _interactionCheckTimer = _manager.interactionCheckTime;
            SetBothSides(me, you);
        }

        public void SetAffinities(float affinity)
        {
            _myAffinityToYou = affinity;
            _impactAccumulationThreshold = 10 / affinity;
            _relationshipAmplifyFactor = Mathf.Abs(affinity - 0.5f);
        }

        public void PassTime(float deltaTime)
        {
            totalInteractionTime += deltaTime;
            _interactionCheckTimer -= deltaTime;

            if (_interactionCheckTimer <= 0)
            {
                _interactionCheckTimer = _manager.interactionCheckTime;
                CheckInteraction();
            }
        }

        private void CheckInteraction()
        {
            ImpactAccumulation();
            RelationshipCheck();
        }

        private void RelationshipCheck()
        {
            if (Random.value < _manager.developRelationFactor * _relationshipAmplifyFactor)
            {
                var meCaptured = me;
                var affinityCaptured = _myAffinityToYou;
                var timeCaptured = totalInteractionTime;
                var relationCaptured = myRelationToYou;
                var relation = Relation.Relation.CheckRelationDevelopment(meCaptured, affinityCaptured,
                    timeCaptured, relationCaptured);
                if (relation == RelationType.Neutral)
                    return;
                AddRelation(relation);
            }
        }

        private void AddRelation(RelationType relation)
        {
            me.AddRelation(you, relation);
            
            if(myRelationToYou is null)
                myRelationToYou = me.Relations[you];

            switch (relation)
            {
                case RelationType.Friend:
                    me.SetOrbitAround(you, Random.Range(_manager.beFriendOrbitTimeMin, _manager.beFriendOrbitTimeMax));
                    break;
                case RelationType.Family:
                    me.SetOrbitAround(you, Random.Range(_manager.beFamilyOrbitTimeMin, _manager.beFamilyOrbitTimeMax), RelationType.Family);
                    break;
                default:
                    break;
            }
        }

        private void ImpactAccumulation()
        {
            _impactAccumulateCounter++;

            if (_impactAccumulateCounter > _impactAccumulationThreshold)
            {
                _impactAccumulateCounter = 0;
                if (Random.value <= _myAffinityToYou)
                {
                    me.AddImpact(you.color, (Random.value <= _myAffinityToYou) ? 2 : 1);
                }
            }
        }

        public void SetBothSides(Unit me, Unit you)
        {
            this.me = me;
            this.you = you;
            me.Relations.TryGetValue(you, out myRelationToYou);
        }

        public void Clear()
        {
            totalInteractionTime = 0;
            me = null;
            you = null;
            myRelationToYou = null;
            _interactionCheckTimer = _manager.interactionCheckTime;
            _myAffinityToYou = 0;
            _impactAccumulationThreshold = 0;
            _impactAccumulateCounter = 0;
            _relationshipAmplifyFactor = 0;
        }
    }
}
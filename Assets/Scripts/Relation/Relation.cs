using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Relation
{
    public enum RelationType
    {
        Neutral,
        Friend,
        Family,
        Enemy
    }
    
    [System.Serializable]
    public class Relation
    {
        public Unit.Unit me;
        public Unit.Unit you;
        public RelationType type;
        
        private enum RelationChangeType
        {
            Improved,
            Nothing,
            Worse
        }
        
        public Relation(Unit.Unit me, Unit.Unit you, RelationType type)
        {
            this.me = me;
            this.you = you;
            this.type = type;
        }

        public static RelationType CheckRelationDevelopment(Unit.Unit me, float myAfftoYou,
            float interactionTime, Relation existedRelation = null)
        {
            switch (ImproveRelationOrNot(myAfftoYou, interactionTime, existedRelation?.type ?? RelationType.Neutral,
                me.NumberOfEachRelation[RelationType.Friend],
                me.NumberOfEachRelation[RelationType.Family], 
                me.NumberOfEachRelation[RelationType.Enemy]))
            {
                case RelationChangeType.Improved:
                    if (existedRelation is null)
                    {
                        return RelationType.Friend;
                    }
                    else
                    {
                        switch (existedRelation.type)
                        {
                            case RelationType.Enemy:
                                return RelationType.Neutral;
                            case RelationType.Friend:
                                return RelationType.Family;
                            case RelationType.Family:
                                return RelationType.Neutral;
                            case RelationType.Neutral:
                                return RelationType.Friend;;
                            default:
                                return RelationType.Neutral;
                        }
                    }
                case RelationChangeType.Worse:
                    if (existedRelation is null)
                    {
                        return RelationType.Enemy;
                    }
                    else
                    {
                        switch (existedRelation.type)
                        {
                            case RelationType.Enemy:
                                return RelationType.Neutral;
                            case RelationType.Friend:
                                return RelationType.Neutral;
                            case RelationType.Family:
                                return RelationType.Enemy;
                            case RelationType.Neutral:
                                return RelationType.Enemy;
                            default:
                                return RelationType.Neutral;
                        }
                    }
                default:
                    return RelationType.Neutral;
            }
        }

        private static RelationChangeType ImproveRelationOrNot(float affinity, float timePassed, RelationType modifier, uint numFriend, uint numFamily, uint numEnemy)
        {
            var chance = affinity * (1 + timePassed / 100);
            
            if (Random.value < chance)
            {
                return modifier switch
                {
                    RelationType.Neutral => Random.value > (float)numFriend / 10 - Mathf.Log(numFriend+1) / 10
                        ? RelationChangeType.Improved
                        : RelationChangeType.Nothing,
                    RelationType.Friend => timePassed / 1000 >= (1 - affinity) * Mathf.Exp(numFamily)
                        ? RelationChangeType.Improved
                        : RelationChangeType.Nothing,
                    RelationType.Enemy => Random.value < 0.1 * (1 + affinity)
                        ? RelationChangeType.Improved
                        : RelationChangeType.Nothing,
                    _ => RelationChangeType.Improved
                };
            }

            if(affinity < .5f && Random.value < .5 + (float)numEnemy / 10 
                - ((timePassed > 7) ? timePassed / 100 : 0) 
                - ((numFriend+numFamily > 5) ? Mathf.Sqrt(numFriend + numFamily) / 23 : 0))
            {
                return modifier switch
                {
                    RelationType.Friend => Random.value < 0.35 * (1 - affinity)
                        ? RelationChangeType.Worse
                        : RelationChangeType.Nothing,
                    _ => RelationChangeType.Worse
                };
            }

            return RelationChangeType.Nothing;
        }
    }
}
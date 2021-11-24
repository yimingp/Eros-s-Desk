namespace Affinities
{
    [System.Serializable]
    public class AffinityData
    {
        public float generalAffinity;
        
        public float collisionImpactRate;
        public int collisionImpactMagnitude;

        public AffinityData()
        {
            
        }

        public void SetCollisionImpactRate(float number)
        {
            collisionImpactRate = generalAffinity * number;
        }
    }
}
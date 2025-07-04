using System;
using System.Collections.Generic;
using UnityEngine;

namespace Class4
{
    [Serializable]
    public class CohesionBehaviour : FlockBehavior
    {
        public override Vector3 GetDirection (Boid boid, List<Boid> neighbors)
        {
            Vector3 cohesionVector = Vector3.zero;

            if (neighbors.Count > 0)
            {
                foreach (Boid b in neighbors)
                    cohesionVector += b.transform.position;

                cohesionVector = (cohesionVector / neighbors.Count) - boid.transform.position;
            }

            cohesionVector *= staticWeight * GetDynamicWeight(boid, cohesionVector);

            return cohesionVector;
        }
    }
}
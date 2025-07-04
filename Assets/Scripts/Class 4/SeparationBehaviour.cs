using System;
using System.Collections.Generic;
using UnityEngine;

namespace Class4
{
    [Serializable]
    public class SeparationBehaviour : FlockBehavior
    {
        [SerializeField, Range(0f, 1f)] private float neighbourRadiusPerc = 0.5f;

        public override Vector3 GetDirection (Boid boid, List<Boid> neighbors)
        {
            Vector3 separationVector = Vector3.zero;
            
            int avoidanceCount = 0;

            foreach (Boid b in neighbors)
            {
                Vector3 diff = boid.transform.position - b.transform.position;

                if (diff.sqrMagnitude <= flock.NeighbourRadiusSquared * neighbourRadiusPerc)
                {
                    separationVector += diff;
                    avoidanceCount++;
                }
            }

            if (avoidanceCount > 0)
                separationVector /= avoidanceCount;

            separationVector *= staticWeight * GetDynamicWeight(boid, separationVector);

            return separationVector;
        }
    }
}
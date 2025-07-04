using System;
using System.Collections.Generic;
using UnityEngine;

namespace Class4
{
    [Serializable]
    public class AlignmentBehaviour : FlockBehavior
    {
        public override Vector3 GetDirection (Boid boid, List<Boid> neighbors)
        {
            Vector3 alignmentVector = Vector3.zero;

            if (neighbors.Count > 0)
            {
                foreach (Boid b in neighbors)
                    alignmentVector += b.transform.forward;

                alignmentVector /= neighbors.Count;
            }
            else
            {
                alignmentVector = boid.transform.forward;
            }

            alignmentVector *= staticWeight * GetDynamicWeight(boid, alignmentVector);

            return alignmentVector;
        }
    }
}
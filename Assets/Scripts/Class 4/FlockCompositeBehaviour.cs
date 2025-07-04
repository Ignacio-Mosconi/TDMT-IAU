using System;
using System.Collections.Generic;
using UnityEngine;

namespace Class4
{
    [Serializable]
    public class FlockCompositeBehaviour
    {
        [SerializeField] private AlignmentBehaviour alignmentBehaviour = default;
        [SerializeField] private CohesionBehaviour cohesionBehaviour = default;
        [SerializeField] private SeparationBehaviour separationBehaviour = default;

        private List<FlockBehavior> flockBehaviors;

        
        public void Initialize(Flock flock)
        {
            flockBehaviors = new List<FlockBehavior>()
            {
                alignmentBehaviour,
                cohesionBehaviour,
                separationBehaviour
            };

            foreach (FlockBehavior flockBehavior in flockBehaviors)
                flockBehavior.Initialize(flock);
        }

        public Vector3 UpdateBoidDirection (Boid boid, List<Boid> neighbors)
        {
            Vector3 direction = Vector3.zero;

            for (int i = 0; i < flockBehaviors.Count; i++)
                direction += flockBehaviors[i].GetDirection(boid, neighbors);

            if (direction == Vector3.zero)
                direction = boid.transform.forward;

            return direction.normalized;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Class4
{
    [Serializable]
    public abstract class FlockBehavior
    {
        [SerializeField, Range(0f, 1f)] protected float staticWeight = 1f;

        protected Flock flock;

        
        public virtual void Initialize (Flock flock)
        {
            this.flock = flock;
        }

        protected float GetDynamicWeight (Boid boid, Vector3 targetDirection)
        {
            float angleBetweenDirs = Vector3.Angle(boid.transform.forward, targetDirection);
            return Mathf.Clamp01(angleBetweenDirs / 180f);
        }

        public abstract Vector3 GetDirection (Boid boid, List<Boid> neighbors);
    }
}
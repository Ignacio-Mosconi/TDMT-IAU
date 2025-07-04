using System.Collections.Generic;
using UnityEngine;

namespace Class4
{
    public class Flock : MonoBehaviour
    {
        [SerializeField] private FlockCompositeBehaviour flockBehaviour;
        [SerializeField] private Boid boidPrefab;
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField, Range(0, 100)] private int boidsCount = 10;
        [SerializeField, Range(1f, 50f)] private float boidsDensity = 2f;
        [SerializeField, Range(1f, 20f)] private float maxBoidsMoveSpeed = 5f;
        [SerializeField, Range(1f, 180f)] private float maxBoidsRotateSpeed = 45f;
        [SerializeField, Range(1f, 10f)] private float neighbourRadius = 2f;

        private List<Boid> flock = new List<Boid>();

        public float NeighbourRadiusSquared => neighbourRadius * neighbourRadius;


        void Start ()
        {
            flockBehaviour.Initialize(this);

            for (int i = 0; i < boidsCount; i++)
            {
                Vector3 spawnPosition = spawnPoint + Random.insideUnitSphere * (boidsCount / boidsDensity);
                Quaternion spawnRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
                Boid boid = Instantiate(boidPrefab, spawnPosition, spawnRotation, transform);

                flock.Add(boid);
            }
        }

        void Update ()
        {
            foreach (Boid boid in flock)
            {
                List<Boid> neighbours = GetNeighbours(boid);
                Vector3 direction = flockBehaviour.UpdateBoidDirection(boid, neighbours);

                boid.Move(direction, maxBoidsMoveSpeed, maxBoidsRotateSpeed);
            }
        }

        private bool AreNeighbours (Boid a, Boid b)
        {
            Vector3 diff = b.transform.position - a.transform.position;
            return diff.sqrMagnitude <= NeighbourRadiusSquared;
        }

        private List<Boid> GetNeighbours (Boid boid)
        {
            List<Boid> neighbours = new List<Boid>();

            foreach (Boid b in flock)
            {
                if (b == boid)
                    continue;

                if (AreNeighbours(boid, b))
                    neighbours.Add(b);
            }

            return neighbours;
        }
    }
}
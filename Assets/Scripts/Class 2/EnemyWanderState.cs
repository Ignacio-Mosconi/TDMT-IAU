using System;
using UnityEngine;
using Class1;

namespace Class2
{
    [Serializable]
    public class EnemyWanderState : FsmState<Enemy>
    {
        [SerializeField, Range(0f, 20f)] private float wanderSpeed = 5f;
        [SerializeField, Range(0f, 20f)] private float searchRadius = 5f;
        [SerializeField, Range(0f, 20f)] private float circleWanderRadius = 2f;
        [SerializeField, Range(4, 64)] private int circleSearchPrecision = 16;

        private PathNodeAgent agent;
        private Vector3 wanderStartPoint;
        private float currentWanderAngle = 0f;


        private Vector3 GetNextDestination ()
        {
            Vector3 nextDestination;

            float radians = currentWanderAngle * Mathf.Deg2Rad;
            float offsetX = Mathf.Cos(radians);
            float offsetZ = Mathf.Sin(radians);

            nextDestination = wanderStartPoint + new Vector3(offsetX, 0f, offsetZ) * circleWanderRadius;

            currentWanderAngle += 360f / circleSearchPrecision;

            if (currentWanderAngle > 360f)
                currentWanderAngle -= 360f;

            Debug.DrawLine(wanderStartPoint + Vector3.up, nextDestination + Vector3.up, Color.magenta, 2f);

            return nextDestination;
        }

        private bool IsInChaseRange ()
        {
            float sqrDistanceToTarget = (owner.AttackTarget.position - owner.transform.position).sqrMagnitude;

            return sqrDistanceToTarget <= searchRadius * searchRadius;
        }

        protected override void OnInitialize ()
        {
            agent = owner.GetComponent<PathNodeAgent>();
        }

        public override void Enter ()
        {
            wanderStartPoint = owner.transform.position;
            agent.MovementSpeed = wanderSpeed;
            agent.Destination = GetNextDestination();
        }

        public override void Update ()
        {
            if (agent.HasReachedDestination)
                agent.Destination = GetNextDestination();

            if (IsInChaseRange())
                owner.OnPlayerDetected?.Invoke();
        }

        public override void Exit ()
        {
            wanderStartPoint = Vector3.zero;
            currentWanderAngle = 0f;
        }
    }
}
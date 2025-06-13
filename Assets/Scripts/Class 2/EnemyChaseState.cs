using System;
using UnityEngine;
using Class1;

namespace Class2
{
    [Serializable]
    public class EnemyChaseState : FsmState<Enemy>
    {
        [SerializeField, Range(0f, 20f)] private float chaseSpeed = 5f;
        [SerializeField, Range(0f, 20f)] private float loseTrackRange = 5f;
        [SerializeField, Range(0f, 1f)] private float pathfindingIntervals = 0.5f;

        private PathNodeAgent agent;
        private float pathfindingTimer;
        private float selfRadius;
        private float targetRadius;

        
        private Vector3 GetTargetPosition ()
        {
            Vector3 targetDir = (owner.AttackTarget.position - owner.transform.position).normalized;

            return owner.AttackTarget.position - targetDir * (selfRadius + targetRadius);
        }

        private bool IsOutOfRange ()
        {
            float sqrDistanceToTarget = (owner.AttackTarget.position - owner.transform.position).sqrMagnitude;

            return sqrDistanceToTarget >= loseTrackRange * loseTrackRange;
        }

        protected override void OnInitialize ()
        {
            agent = owner.GetComponent<PathNodeAgent>();
            selfRadius = owner.GetComponentInChildren<CapsuleCollider>().radius;
            targetRadius = owner.AttackTarget.GetComponentInChildren<CapsuleCollider>().radius;
        }

        public override void Enter ()
        {
            agent.MovementSpeed = chaseSpeed;
            agent.Destination = GetTargetPosition();
        }

        public override void Update ()
        {
            pathfindingTimer += Time.deltaTime;

            if (pathfindingTimer >= pathfindingIntervals)
            {
                agent.Destination = GetTargetPosition();
                pathfindingTimer -= pathfindingIntervals;
            }

            if (IsOutOfRange())
                owner.OnPlayerOutOfRange?.Invoke();
        }

        public override void Exit () 
        {
            pathfindingTimer = 0f;
        }
    }
}
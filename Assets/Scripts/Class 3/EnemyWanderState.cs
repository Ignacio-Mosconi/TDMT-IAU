using System;
using UnityEngine;
using UnityEngine.AI;

namespace Class3
{
    public class EnemyWanderState : StateMachineBehaviour
    {
        [SerializeField, Range(0f, 20f)] private float wanderSpeed = 5f;
        [SerializeField, Range(0f, 20f)] private float searchRadius = 5f;
        [SerializeField, Range(0f, 20f)] private float circleWanderRadius = 2f;
        [SerializeField, Range(4, 64)] private int circleSearchPrecision = 16;

        private NavMeshAgent agent;
        private Transform attackTarget;
        private Vector3 wanderStartPoint;
        private float currentWanderAngle = 0f;


        private Vector3 GetNextDestination()
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

        private bool IsInChaseRange()
        {
            float sqrDistanceToTarget = (attackTarget.position - agent.transform.position).sqrMagnitude;

            return sqrDistanceToTarget <= searchRadius * searchRadius;
        }

        public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!agent)
                agent = animator.GetComponent<NavMeshAgent>();

            if (!attackTarget)
                attackTarget = FindAnyObjectByType<PlayerClickMovement>().transform;

            wanderStartPoint = agent.transform.position;
            agent.speed = wanderSpeed;
            agent.destination = GetNextDestination();
        }

        public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
                agent.destination = GetNextDestination();

            if (IsInChaseRange())
                animator.SetTrigger("PlayerDetected");
        }

        public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            wanderStartPoint = Vector3.zero;
            currentWanderAngle = 0f;
        }
    }
}
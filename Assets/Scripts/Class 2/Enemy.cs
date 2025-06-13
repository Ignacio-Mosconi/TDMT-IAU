using UnityEngine;
using UnityEngine.Events;
using Class1;

namespace Class2
{
    [RequireComponent(typeof(PathNodeAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private EnemyWanderState wanderState = new EnemyWanderState();
        [SerializeField] private EnemyChaseState chaseState = new EnemyChaseState();

        [Header("Common References")]
        [SerializeField] private Transform attackTarget;

        private FiniteStateMachine<Enemy> fsm;
        
        public Transform AttackTarget => attackTarget;

        public UnityEvent OnPlayerDetected { get; private set; } = new UnityEvent();
        public UnityEvent OnPlayerOutOfRange { get; private set; } = new UnityEvent();


        void Awake ()
        {
            wanderState.Initialize(this);
            chaseState.Initialize(this);
        }

        void Start ()
        {
            FsmState<Enemy>[] states = { wanderState, chaseState };
            UnityEvent[] events = { OnPlayerDetected, OnPlayerOutOfRange };
            
            fsm = new FiniteStateMachine<Enemy>(states, events, wanderState);

            fsm.ConfigureTransition(wanderState, chaseState, OnPlayerDetected);
            fsm.ConfigureTransition(chaseState, wanderState, OnPlayerOutOfRange);
        }

        void Update ()
        {
            fsm.Update();
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace Class3
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerClickMovement : MonoBehaviour
    {
        private Camera mainCamera;
        private NavMeshAgent agent;


        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;

                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, maxDistance: float.MaxValue))
                    agent.destination = raycastHit.point;
            }
        }
    }
}
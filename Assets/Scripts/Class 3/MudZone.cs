using UnityEngine;
using UnityEngine.AI;

namespace Class3
{
    public class MudZone : MonoBehaviour
    {
        [SerializeField] private float speedReductionPerc = 0.5f;
        [SerializeField] private LayerMask affectedLayers;


        private bool IsAffectedLayer (int layerNumber)
        {
            string layerName = LayerMask.LayerToName(layerNumber);
            int mask = LayerMask.GetMask(layerName);

            return (affectedLayers.value & mask) != 0;
        }


        void OnTriggerEnter (Collider other)
        {
            NavMeshAgent agent = other.gameObject.GetComponentInParent<NavMeshAgent>();

            if (agent && IsAffectedLayer(agent.gameObject.layer))
                agent.speed *= speedReductionPerc;
        }
        
        void OnTriggerExit (Collider other)
        {
            NavMeshAgent agent = other.gameObject.GetComponentInParent<NavMeshAgent>();

            if (agent && IsAffectedLayer(agent.gameObject.layer))
                agent.speed /= speedReductionPerc;
        }
    }
}
using UnityEngine;

namespace Class1
{
    public class MudZone : MonoBehaviour
    {
        [SerializeField] private float speedReductionPerc = 0.5f;
        [SerializeField] private LayerMask affectedLayers;


        void OnTriggerEnter (Collider other)
        {
            PathNodeAgent agent = other.gameObject.GetComponentInParent<PathNodeAgent>();

            if (agent && (affectedLayers.value & agent.gameObject.layer) != 0)
                agent.MovementSpeed *= speedReductionPerc;
        }
        
        void OnTriggerExit (Collider other)
        {
            PathNodeAgent agent = other.gameObject.GetComponentInParent<PathNodeAgent>();

            if (agent && (affectedLayers.value & agent.gameObject.layer) != 0)
                agent.MovementSpeed /= speedReductionPerc;
        }
    }
}
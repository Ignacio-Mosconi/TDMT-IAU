using UnityEngine;

namespace Class3
{
    public class MovingObstacle : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float moveSpeed = 2f;
        [SerializeField, Range(0f, 10f)] private float moveDistance = 5f;

        private Vector3 initialPosition;
        private Vector3 targetPosition;


        void Awake ()
        {
            initialPosition = transform.position;
            targetPosition = transform.position + transform.forward * moveDistance;
        }

        void Update ()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
                (initialPosition, targetPosition) = (targetPosition, initialPosition);
        }
    }
}
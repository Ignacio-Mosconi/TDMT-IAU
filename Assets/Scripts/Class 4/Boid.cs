using UnityEngine;

namespace Class4
{
    public class Boid : MonoBehaviour
    {
        public void Move (Vector3 targetDirection, float maxMoveSpeed, float maxRotationSpeed)
        {
            float moveDelta = maxMoveSpeed * Time.deltaTime;
            float maxRadiansDelta = maxRotationSpeed * Mathf.Deg2Rad * Time.deltaTime;

            targetDirection.Normalize();

            transform.forward = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta, 0f);
            transform.position += targetDirection * moveDelta;
        }
    }
}
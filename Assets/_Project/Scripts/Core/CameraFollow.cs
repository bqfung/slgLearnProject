using UnityEngine;

namespace SLGLearn.Core
{
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new(0f, 7f, -8f);
        [SerializeField, Min(0f)] private float followSpeed = 8f;
        [SerializeField] private bool lookAtTarget = true;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                1f - Mathf.Exp(-followSpeed * Time.deltaTime));

            if (lookAtTarget)
            {
                transform.LookAt(target.position);
            }
        }

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private float deadZone = 1.5f;

    private Transform target;

    private void LateUpdate()
    {
        if (target == null)
            target = GameObject.FindWithTag("Player")?.transform;

        if (target == null) return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        float distance = Vector2.Distance(transform.position, targetPos);

        if (distance > deadZone)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            float moveAmount = (distance - deadZone) * smoothSpeed * Time.deltaTime;
            transform.position += direction * moveAmount;
        }
    }
}

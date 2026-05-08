using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private float deadZone = 1.5f;

    [Header("Limites de camara")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX =  20f;
    [SerializeField] private float minY = -15f;
    [SerializeField] private float maxY =  15f;

    private Transform target;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null && Player.Instance != null)
            target = Player.Instance.transform;

        if (target == null) return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        float distance = Vector2.Distance(transform.position, targetPos);

        if (distance > deadZone)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            float moveAmount = (distance - deadZone) * smoothSpeed * Time.deltaTime;
            transform.position += direction * moveAmount;
        }

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        Vector3 pos = transform.position;
        if (minX + halfW < maxX - halfW)
            pos.x = Mathf.Clamp(pos.x, minX + halfW, maxX - halfW);
        if (minY + halfH < maxY - halfH)
            pos.y = Mathf.Clamp(pos.y, minY + halfH, maxY - halfH);
        transform.position = pos;
    }
}

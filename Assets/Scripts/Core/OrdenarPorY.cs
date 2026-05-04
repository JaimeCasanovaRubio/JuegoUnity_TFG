using UnityEngine;

public class OrdenarPorY : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        sr.sortingOrder = -(int)(transform.position.y * 10);
    }
}

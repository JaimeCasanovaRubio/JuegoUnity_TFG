using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSortByY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public int sortingOrderOffset = 0;

    public bool runOnlyOnce = false;

    // Número base grande para evitar números negativos con posiciones altas en Y
    private const int BaseSortingOrder = 5000;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // Y positivo = Arriba (debería ir detrás, menor SortingOrder)
        // Y negativo = Abajo (debería ir delante, mayor SortingOrder)
        // Multiplicamos por 100 para dar margen a cambios pequeños en Y
        spriteRenderer.sortingOrder = (int)(BaseSortingOrder - transform.position.y * 100) + sortingOrderOffset;

        if (runOnlyOnce)
        {
            Destroy(this); // Se borra el script si no hace falta calcularlo cada frame
        }
    }
}

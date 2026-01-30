using UnityEngine;

/// <summary>
/// Clase base abstracta para todos los items coleccionables.
/// Equivalente a Item.java de LibGDX.
/// En Unity, los items usan componentes Collider2D como trigger.
/// </summary>
public abstract class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] protected bool isCollectable = true;
    protected bool isCollected = false;

    protected SpriteRenderer spriteRenderer;
    protected Collider2D itemCollider;

    public bool IsCollected => isCollected;
    public bool IsCollectable => isCollectable;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();

        // Asegurar que el collider es un trigger
        if (itemCollider != null)
        {
            itemCollider.isTrigger = true;
        }
    }

    /// <summary>
    /// Aplica el efecto del item al jugador.
    /// Las clases hijas implementan su propio efecto.
    /// </summary>
    public abstract void ApplyEffect(Player player);

    /// <summary>
    /// Llamado cuando el jugador entra en el trigger del item.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && isCollectable && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Collect(player);
            }
        }
    }

    /// <summary>
    /// Recoge el item y aplica su efecto.
    /// </summary>
    protected virtual void Collect(Player player)
    {
        if (!isCollected)
        {
            ApplyEffect(player);
            isCollected = true;
            Debug.Log($"✓ {gameObject.name} recogido!");

            // Desactivar o destruir el item
            OnCollected();
        }
    }

    /// <summary>
    /// Llamado después de recoger el item.
    /// Por defecto destruye el GameObject.
    /// </summary>
    protected virtual void OnCollected()
    {
        // Podrías añadir una animación o efecto de partículas aquí
        Destroy(gameObject);
    }
}

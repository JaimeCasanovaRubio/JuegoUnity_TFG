using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private string ownerTag;

    private Rigidbody2D rb;

    private Collider2D[] projColliders;

    private void Awake()
    {
        // Desactivamos los colliders al nacer para evitar que detecten colisiones 
        // antes de que Setup() configure quién es el dueño.
        projColliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in projColliders)
        {
            c.enabled = false;
        }
    }

    public void Setup(Vector2 dir, float spd, float dmg, GameObject owner)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        
        if (owner != null)
        {
            ownerTag = owner.tag;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
            Debug.Log($"[Projectile] Disparado con velocidad: {rb.linearVelocity} | Speed: {speed}");
        }
        else 
        {
            Debug.LogError("[Projectile] No tiene Rigidbody2D!");
        }

        // Ignorar físicamente todas las colisiones con el creador y sus objetos hijos
        if (owner != null && projColliders != null)
        {
            Collider2D[] ownerColliders = owner.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D pc in projColliders)
            {
                foreach (Collider2D oc in ownerColliders)
                {
                    Physics2D.IgnoreCollision(pc, oc);
                }
            }
        }

        // Ahora que ya ignora al dueño, activamos los colliders
        foreach (Collider2D c in projColliders)
        {
            c.enabled = true;
        }

        // Destruir el proyectil después de 1 segundo como máximo para que no dure infinito
        Destroy(gameObject, 1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        // Evitar el error "Tag is null or empty" si ownerTag no se ha inicializado
        if (!string.IsNullOrEmpty(ownerTag) && collision.gameObject.CompareTag(ownerTag)) 
        {
            return;
        }

        // Si choca con una entidad (como un Enemigo), le hacemos daño
        Entity entity = collision.gameObject.GetComponentInChildren<Entity>();
        if (entity != null)
        {
            if(ownerTag == "Player"){
                Player player = Player.Instance;
                Enemie enemie = collision.gameObject.GetComponentInChildren<Enemie>();
                if(enemie != null){
                    Debug.Log("[Projectile] Chocó con un enemigo");
                    enemie.TakeDamage(damage, player.Afinidad);
                }
               
            }else{
                entity.TakeDamage(damage);
            }
            
        }

        // Se destruye al chocar contra cualquier cosa (árboles, enemigos, paredes...)
        Debug.Log("El proyectil chocó al instante contra: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}

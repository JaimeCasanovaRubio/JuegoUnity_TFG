using UnityEngine;

public class RangedEnemy : Enemie
{
    [Header("Ranged Attack Parameters")]
    [SerializeField] private float rangeOfAttack = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;

    protected override void HandleMovement()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= rangeOfAttack)
        {
            state = EnemyState.Attack;
        }
        else if (distanceToPlayer <= rangeOfDetection)
        {
            state = EnemyState.Chase;
        }
        else
        {
            state = EnemyState.Patrol;
        }

        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                rb.linearVelocity = Vector2.zero; // Detenerse para disparar
                Attack(player);
                break;
        }
    }

    protected override void Attack(Player target)
    {
        if (target != null)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                ShootProjectile(target);
            }
        }
    }

    private void ShootProjectile(Player target)
    {
        if (projectilePrefab != null)
        {
            Vector3 spawnPos = transform.position;
            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            
            Vector2 direction = (target.transform.position - spawnPos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Projectile projectileScript = proj.GetComponentInChildren<Projectile>();
            if (projectileScript == null) 
            {
                projectileScript = proj.GetComponent<Projectile>();
            }

            if (projectileScript != null)
            {
                projectileScript.Setup(direction, projectileSpeed, damage, gameObject);
            }
            else
            {
                Debug.LogError("¡ATENCIÓN! El prefab disparado no tiene el script Projectile.cs asociado.");
            }
        }
        else 
        {
            Debug.LogWarning("Projectile Prefab no asignado en " + gameObject.name);
        }
    }
}

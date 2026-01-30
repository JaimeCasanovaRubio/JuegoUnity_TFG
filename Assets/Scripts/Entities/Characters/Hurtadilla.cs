using UnityEngine;

/// <summary>
/// Personaje Hurtadilla con habilidad de Dash.
/// Equivalente a Hurtadilla.java de LibGDX.
/// </summary>
public class Hurtadilla : Player
{
    [Header("Dash Settings")]
    [SerializeField] private float dashMultiplier = 4f;

    protected override void Awake()
    {
        base.Awake();

        // Configurar stats específicos de Hurtadilla
        abilityCooldown = 1f;
        abilityDuration = 0.1f;
    }

    /// <summary>
    /// Habilidad de Hurtadilla: DASH
    /// Se mueve rápidamente en la última dirección.
    /// </summary>
    protected override void ExecuteAbility()
    {
        // Activar movimiento de habilidad
        abilityAffectsMovement = true;

        // Configurar la dirección del dash
        switch (lastDirection)
        {
            case "right":
                abilityMoveDirection = Vector2.right * dashMultiplier * speed;
                break;
            case "left":
                abilityMoveDirection = Vector2.left * dashMultiplier * speed;
                break;
            case "up":
                abilityMoveDirection = Vector2.up * dashMultiplier * speed;
                break;
            case "down":
                abilityMoveDirection = Vector2.down * dashMultiplier * speed;
                break;
            default:
                abilityMoveDirection = (facingRight ? Vector2.right : Vector2.left) * dashMultiplier * speed;
                break;
        }

        Debug.Log($"💨 Hurtadilla hace DASH hacia {lastDirection}!");
    }

    protected override void OnAbilityEnd()
    {
        base.OnAbilityEnd();
        // El dash ha terminado
        Debug.Log("Dash terminado");
    }
}

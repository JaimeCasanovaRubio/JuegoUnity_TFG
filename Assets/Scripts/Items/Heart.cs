using UnityEngine;

/// <summary>
/// Item que recupera vida del jugador.
/// Equivalente a Heart.java de LibGDX.
/// </summary>
public class Heart : Item
{
    [Header("Heart Settings")]
    [SerializeField] private int healAmount = 1;

    public override void ApplyEffect(Player player)
    {
        player.Heal(healAmount);
        Debug.Log($"💖 +{healAmount} vida");
    }
}

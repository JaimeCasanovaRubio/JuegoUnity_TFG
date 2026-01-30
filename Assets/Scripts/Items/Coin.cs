using UnityEngine;

/// <summary>
/// Item que da puntos al jugador.
/// Equivalente a Coin.java de LibGDX.
/// </summary>
public class Coin : Item
{
    [Header("Coin Settings")]
    [SerializeField] private int value = 10;

    // Evento estático para que otros sistemas puedan escuchar cuando se recoge una moneda
    public static event System.Action<int> OnCoinCollected;

    public override void ApplyEffect(Player player)
    {
        // Notificar a cualquier sistema de puntuación
        OnCoinCollected?.Invoke(value);
        Debug.Log($"🪙 +{value} puntos");
    }
}

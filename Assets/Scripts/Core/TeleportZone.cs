using UnityEngine;

/// <summary>
/// Zona de teletransporte que lleva al jugador a otra escena.
/// Colocar este script en objetos con tag "TP".
/// </summary>
public class TeleportZone : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("Nombre de la escena a la que teletransporta")]
    [SerializeField] private string targetScene = "OniricForest";

    /// <summary>
    /// Escena destino del teletransporte.
    /// </summary>
    public string TargetScene => targetScene;
}
